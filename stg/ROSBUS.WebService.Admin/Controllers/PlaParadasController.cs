using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class PlaParadasController : ManagerController<PlaParadas, int, PlaParadasDto, PlaParadasFilter, IPlaParadasAppService>
    {
        public PlaParadasController(IPlaParadasAppService service)
            : base(service)
        {
           
        }

        public async override Task<IActionResult> GetAllAsync(PlaParadasFilter filter)
        {
            try
            {
    
                var pList = await this.Service.GetDtoAllFilterAsync(filter);

                return ReturnData<PagedResult<PlaParadasDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<PlaParadasDto>>(ex);
            }
        }

       


    }


 

}
