using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class InvolucradosAppService : AppServiceBase<SinInvolucrados, InvolucradosDto, int, IInvolucradosService>, IInvolucradosAppService
    {
        private readonly ILocalidadesService _localidaddervice;
        private readonly IAdjuntosService _adjuntosService;
        private readonly IConductoresService _conductoresService;


        public InvolucradosAppService(IInvolucradosService serviceBase, ILocalidadesService localidaddervice, IAdjuntosService adjuntosService, IConductoresService conductoresService, IReclamosService reclamosService)
            : base(serviceBase)
        {
            _localidaddervice = localidaddervice;
            _adjuntosService = adjuntosService;
            _conductoresService = conductoresService;
            _reclamosService = reclamosService;

        }
        private readonly IReclamosService _reclamosService;
        public async Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc)
        {
            return await _serviceBase.HistorialSiniestros(TipoDocId, NroDoc);
        }
        public override async Task<InvolucradosDto> AddAsync(InvolucradosDto dto)
        {
            dto.LocalidadId = dto.selectLocalidades?.Id;

            if (dto.Conductor != null)
                dto.Conductor.LocalidadId = dto.Conductor?.selectLocalidades?.Id;

            if (dto.MuebleInmueble != null)
                dto.MuebleInmueble.LocalidadId = (dto.MuebleInmueble?.selectLocalidades?.Id).GetValueOrDefault();

            foreach (var item in dto.DetalleLesion)
            {
                if (item.Id < 0)
                    item.Id = 0;
            }

            return await base.AddAsync(dto);
        }

        public override async Task<InvolucradosDto> UpdateAsync(InvolucradosDto dto)
        {


            dto.LocalidadId = dto.selectLocalidades?.Id;

            if (dto.Conductor != null)
            {
                dto.Conductor.LocalidadId = dto.Conductor?.selectLocalidades?.Id;
                if ((int)dto.Conductor.Id == 0)
                {
                    SinConductores cond = new SinConductores();
                    MapObject(dto.Conductor, cond);
                    cond.Id = 0;
                    var condu = await this._conductoresService.AddAsync(cond);
                    dto.ConductorId = condu.Id;
                    dto.Conductor = null;
                }
            }

            if (dto.MuebleInmueble != null)
                dto.MuebleInmueble.LocalidadId = (dto.MuebleInmueble?.selectLocalidades?.Id).GetValueOrDefault();

            foreach (var item in dto.DetalleLesion)
            {
                if (item.Id < 0)
                    item.Id = 0;
            }


            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);

            //var reclamos = await _reclamosService.GetByInvolucrado(dto.Id);
            //entity.SinReclamos = reclamos;

            await this.UpdateAsync(entity);
            return MapObject<SinInvolucrados, InvolucradosDto>(entity);

        }


        public override async Task<PagedResult<InvolucradosDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            var data = await base.GetDtoPagedListAsync(filter);
            var localidades = await _localidaddervice.GetAllLocalidades();

            var items = data.Items.ToList();

            foreach (var dto in items)
            {
                var loc = localidades.Items.FirstOrDefault(e => e.Id == dto.LocalidadId);

                if (loc != null)
                    dto.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);

                if (dto.Conductor != null)
                {
                    loc = localidades.Items.FirstOrDefault(e => e.Id == dto.Conductor.LocalidadId);

                    if (loc != null)
                        dto.Conductor.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);
                }

                if (dto.MuebleInmueble != null)
                {
                    loc = localidades.Items.FirstOrDefault(e => e.Id == dto.MuebleInmueble.LocalidadId);

                    if (loc != null)
                        dto.MuebleInmueble.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);
                }
            }

            return new PagedResult<InvolucradosDto>(data.TotalCount, items);
        }



        public override async Task<InvolucradosDto> GetDtoByIdAsync(int id)
        {
            var localidades = await _localidaddervice.GetAllLocalidades();

            var dto = await base.GetDtoByIdAsync(id);

            var loc = localidades.Items.FirstOrDefault(e => e.Id == dto.LocalidadId);

            if (loc != null)
                dto.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);

            if (dto.Conductor != null)
            {
                loc = localidades.Items.FirstOrDefault(e => e.Id == dto.Conductor.LocalidadId);

                if (loc != null)
                    dto.Conductor.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);
            }

            if (dto.MuebleInmueble != null)
            {
                loc = localidades.Items.FirstOrDefault(e => e.Id == dto.MuebleInmueble.LocalidadId);

                if (loc != null)
                    dto.MuebleInmueble.selectLocalidades = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(loc.Id, loc.DscLocalidad);
            }

            return dto;
        }

        public async Task<List<AdjuntosDto>> GetAdjuntos(int involucradoId)
        {
            List<ItemDto<Guid>> adjuntos = new List<ItemDto<Guid>>();

            List<SinInvolucradosAdjuntos> sinAdj = await this._serviceBase.GetAdjuntos(involucradoId);



            AdjuntosFilter filter = new AdjuntosFilter();
            filter.Ids = sinAdj.Select(e => e.AdjuntoId).ToList(); ;

            adjuntos = await _adjuntosService.GetAdjuntosItemDto(filter);

            return adjuntos.Select(e => new AdjuntosDto() { Id = e.Id, Nombre = e.Description }).ToList();
        }

        public async Task AgregarAdjuntos(int involucradoId, List<AdjuntosDto> result)
        {
            var allEntity = await this._serviceBase.GetAllAsync(e => e.Id == involucradoId);

            var entity = allEntity.Items.FirstOrDefault();
            if (entity != null)
            {
                foreach (var item in result)
                {
                    entity.SinInvolucradosAdjuntos.Add(new SinInvolucradosAdjuntos() { AdjuntoId = item.Id, InvolucradoId = involucradoId });
                }

            }

            await this._serviceBase.UpdateAsync(entity);
        }

        public Task DeleteFileById(Guid id)
        {
            return this._serviceBase.DeleteFileById(id);
        }
    }
}
