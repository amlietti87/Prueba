using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Operaciones.AppService
{

    public class LegajosEmpleadoAppService : AppServiceBase<LegajosEmpleado, LegajosEmpleadoDto, int, ILegajosEmpleadoService>, ILegajosEmpleadoAppService
    {
        public LegajosEmpleadoAppService(ILegajosEmpleadoService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list = await this.GetPagedListAsync(filter);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }


        public async  Task<LegajosEmpleado> GetMaxById(int id)
        {
            return await _serviceBase.GetMaxById(id);
        }
    }
}
