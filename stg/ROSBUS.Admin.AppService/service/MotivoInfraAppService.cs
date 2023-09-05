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
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class MotivoInfraAppService : AppServiceBase<MotivoInfra, MotivoInfraDto, String, IMotivoInfraService>, IMotivoInfraAppService
    {
        public MotivoInfraAppService(IMotivoInfraService serviceBase) 
            :base(serviceBase)
        {
         
        }
        public override async Task<List<ItemDto<string>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var motivos =  await base.GetItemsAsync(filter);
            motivos = motivos.OrderBy(m => m.Description).ToList();

            return motivos;
        }
    }
}
