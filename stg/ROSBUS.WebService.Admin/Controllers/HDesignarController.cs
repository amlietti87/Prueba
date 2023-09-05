using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HDesignarController : ManagerController<HDesignar, int, HDesignarDto, HDesignarFilter, IHDesignarAppService>
    {
        public HDesignarController(IHDesignarAppService service)
            : base(service)
        {

        }

        [HttpGet]
        public async Task<IActionResult> RecuperarSabanaPorSector (HDesignarFilter Filter)
        {
            try
            {
                var sectores = await this.Service.RecuperarSabanaPorSector(Filter);
                return ReturnData<List<HDesignarSabanaSector>>(sectores);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<HDesignarSabanaSector>>(ex);
            }
        }

    }


 

}
