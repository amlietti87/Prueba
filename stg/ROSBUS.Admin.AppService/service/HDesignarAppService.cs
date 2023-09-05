using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HDesignarAppService : AppServiceBase<HDesignar, HDesignarDto, int, IHDesignarService>, IHDesignarAppService
    {
        public HDesignarAppService(IHDesignarService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async Task<List<HDesignarSabanaSector>> RecuperarSabanaPorSector(HDesignarFilter Filter)
        {
            var sectores = await _serviceBase.RecuperarSabanaPorSector(Filter);

            return sectores;
        }

    }
}
