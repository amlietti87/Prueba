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
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class LocalidadesAppService : AppServiceBase<Localidades, LocalidadesDto, int, ILocalidadesService>, ILocalidadesAppService
    {

        public LocalidadesAppService(ILocalidadesService serviceBase) 
            :base(serviceBase)
        {
         
        }


        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list =  await this.GetPagedListAsync(filter);             
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;            
        }

        public async Task<PagedResult<Localidades>> GetAllLocalidades()
        {

            var result = await this._serviceBase.GetAllLocalidades();


            return result;
        }

    }
}
