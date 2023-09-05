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

    public class CoordenadasAppService : AppServiceBase<PlaCoordenadas, CoordenadasDto, int, ICoordenadasService>, ICoordenadasAppService
    {
        public CoordenadasAppService(ICoordenadasService serviceBase, ILocalidadesService localidaddervice)
            : base(serviceBase)
        {
            _localidaddervice = localidaddervice;
        }

        public ILocalidadesService _localidaddervice { get; }




        public async override Task<CoordenadasDto> GetDtoByIdAsync(int id)
        {
            var item = await base.GetDtoByIdAsync(id);

            var localidades = await _localidaddervice.GetAllLocalidades();
            if (item.LocalidadId.HasValue)
            {
                item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
            }
            if (String.IsNullOrWhiteSpace(item.Localidad))
            {
                item.LocalidadId = null;
            }
            return item;
        }


        public async override Task<PagedResult<CoordenadasDto>> GetDtoAllAsync(Expression<Func<PlaCoordenadas, bool>> predicate, List<Expression<Func<PlaCoordenadas, object>>> includeExpression = null)
        {
            var localidades = await _localidaddervice.GetAllLocalidades();
            var result = await base.GetDtoAllAsync(predicate, includeExpression);

            foreach (var item in result.Items)
            {
                if (item.LocalidadId.HasValue)
                {
                    item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
                }

            }

            return result;
        }


        public override async Task<PagedResult<CoordenadasDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {

            var localidades = await _localidaddervice.GetAllLocalidades();
            var result = await base.GetDtoPagedListAsync(filter);

            foreach (var item in result.Items)
            {
                if (item.LocalidadId.HasValue)
                    item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
            }

            return result;
        }

        public override async Task DeleteAsync(int id)
        {
            var coordenadafull = await _serviceBase.GetAllAsync(e=> e.Id == id, new List<Expression<Func<PlaCoordenadas, object>>> { e=> e.HDetaminxtipo, e=> e.HProcMin, e=> e.HSectores, e=> e.PlaPuntos });
            if (coordenadafull.Items.Any(e=> e.HDetaminxtipo.Count > 0 || e.HProcMin.Count > 0 || e.HSectores.Count > 0 || e.PlaPuntos.Count > 0))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("No se puede eliminar la coordenada porque está relacionada a otras tablas");
            }
            await base.DeleteAsync(id);
        }


        public override Task<CoordenadasDto> AddAsync(CoordenadasDto dto)
        {
            dto.Abreviacion = String.Format("{0}-{1}", dto.Calle1, dto.Calle2);
            dto.CodigoNombre = string.Empty;
            dto.BeforeMigration = false;
            return base.AddAsync(dto);
        }

        public override Task<CoordenadasDto> UpdateAsync(CoordenadasDto dto)
        {
            dto.Abreviacion = String.Format("{0}-{1}", dto.Calle1, dto.Calle2);
            dto.CodigoNombre = string.Empty;
            return base.UpdateAsync(dto);
        }


        public async Task<List<PlaCoordenadas>> RecuperarCoordenadasPorFecha(CoordenadasFilter filter)
        {
            var coordenadas = await _serviceBase.RecuperarCoordenadasPorFecha(filter);

            return coordenadas;
        }

    }
}
