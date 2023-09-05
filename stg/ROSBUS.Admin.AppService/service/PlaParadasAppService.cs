using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class PlaParadasAppService : AppServiceBase<PlaParadas, PlaParadasDto, int, IPlaParadasService>, IPlaParadasAppService
    {
        public PlaParadasAppService(IPlaParadasService serviceBase, ILocalidadesService localidaddervice, IRutasService rutasService)
            : base(serviceBase)
        {

            _localidaddervice = localidaddervice;
            _rutasService = rutasService;
        }
        public ILocalidadesService _localidaddervice { get; }

        private IRutasService _rutasService;

        public async override Task<PlaParadasDto> GetDtoByIdAsync(int id)
        {
            var item = await base.GetDtoByIdAsync(id);

            var localidades = await _localidaddervice.GetAllLocalidades();

            item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
            return item;
        }

        public async override Task<PagedResult<PlaParadasDto>> GetDtoAllAsync(Expression<Func<PlaParadas, bool>> predicate, List<Expression<Func<PlaParadas, object>>> includeExpression = null)
        {
            var result = await base.GetDtoAllAsync(predicate, includeExpression);

            await CompletarLocalidad(result);

            return result;
        }

        public async override Task<PagedResult<PlaParadasDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            PlaParadasFilter paradasFilter = filter as PlaParadasFilter;

            if (paradasFilter !=null && paradasFilter.SoloParadasAsociadasALineas.GetValueOrDefault())
            {
                DateTime fecha = paradasFilter.Fecha?.Date ?? DateTime.Now.Date;
                var rutas = await _rutasService.GetAllAsync(e => e.Bandera.HBasec.Any(hb => hb.CodHfechaNavigation.CodLin == paradasFilter.LineaId) 
                                                                                         && e.Fecha.Date <= fecha 
                                                                                         && (e.FechaVigenciaHasta >= fecha || e.FechaVigenciaHasta == null)
                                                                                         //&& e.EstadoRutaId == PlaEstadoRuta.APROBADO 
                                                                                         && e.IsDeleted == false);


                paradasFilter.Rutas = rutas.Items.Select(e => e.Id).ToList();

            }

            var result = await base.GetDtoPagedListAsync(filter);
            await CompletarLocalidad(result);

            return result;
        }

        private async Task CompletarLocalidad(PagedResult<PlaParadasDto> result)
        {
            var localidades = await _localidaddervice.GetAllLocalidades();
            foreach (var item in result.Items)
            {
                if (item.LocalidadId != 0)
                    item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
            }
        }

        public async override Task<PagedResult<PlaParadasDto>> GetDtoAllFilterAsync<TFilter>(TFilter filter)
        {
            var f = filter as PlaParadasFilter;
            if (f.Lat.HasValue && f.Lat.HasValue)
            {
                var listDto = this.MapList<PlaParadas, PlaParadasDto>(await this._serviceBase.ParadasBuscarCercanos(f));

                var result = new PagedResult<PlaParadasDto>(listDto.Count(), listDto.ToList());
                await CompletarLocalidad(result);
                return result;
            }
            else {
                var result = await base.GetDtoAllFilterAsync(filter);
                return result;
            } 

        }


        public override Task<PagedResult<PlaParadas>> GetPagedListAsync<TFilter>(TFilter filter)
        { 
            return base.GetPagedListAsync(filter);
        }


    }
}
