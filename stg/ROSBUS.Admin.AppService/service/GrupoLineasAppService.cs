using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class GrupoLineasAppService : AppServiceBase<PlaGrupoLineas, GrupoLineasDto, int, IGrupoLineasService>, IGrupoLineasAppService
    {
        protected readonly ILineaService _lineaservice;


        public GrupoLineasAppService(IGrupoLineasService serviceBase, ILineaService lineaservice)
            : base(serviceBase)
        {
            _lineaservice = lineaservice;
        }


        public async override Task<GrupoLineasDto> AddAsync(GrupoLineasDto dto)
        {

            var entity = MapObject<GrupoLineasDto, PlaGrupoLineas>(dto);

            entity = await this.AddAsync(entity);

            
            foreach (var item in dto.Lineas.Where(e => e.IsSelected = true))
            {
                var le = _lineaservice.GetById(item.Id);
                le.PlaGrupoLineaId = entity.Id;
                await _lineaservice.UpdateAsync(le);
            }

            return MapObject<PlaGrupoLineas, GrupoLineasDto>(entity);
        }

        public async override Task<GrupoLineasDto> UpdateAsync(GrupoLineasDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);            


            await this.UpdateAsync(entity);

            
            foreach (var item in dto.Lineas.Where(e => e.IsSelected = true))
            {
                var le = _lineaservice.GetById(item.Id);
                le.PlaGrupoLineaId = entity.Id;
                await _lineaservice.UpdateAsync(le);
            }


            return MapObject<PlaGrupoLineas, GrupoLineasDto>(entity);
        }


    }
}
