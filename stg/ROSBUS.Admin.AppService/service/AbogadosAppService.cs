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
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class AbogadosAppService : AppServiceBase<SinAbogados, AbogadosDto, int, IAbogadosService>, IAbogadosAppService
    {
        public AbogadosAppService(IAbogadosService serviceBase, ILocalidadesService localidaddervice) 
            :base(serviceBase)
        {
            _localidaddervice = localidaddervice;
        }
        public ILocalidadesService _localidaddervice { get; }
        public override async Task<PagedResult<AbogadosDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {

            var localidades = await _localidaddervice.GetAllLocalidades();
            var result = await base.GetDtoPagedListAsync(filter);

            foreach (var item in result.Items)
            {
                item.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
            }

            return result;
        }
    }
}
