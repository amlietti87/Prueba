using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.ApiServices.Model;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Mobile.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class LineaController : TECSO.FWK.ApiServices.ControllerBase
    {        

        public LineaController(ILineaAppService _service)           
        {
            this.Service = _service;
        }

        public ILineaAppService Service { get; }

        [HttpGet]
        public async Task<IActionResult> GetLineasPorUsuario()
        {
            try
            {
                var r = await this.Service.GetLineasPorUsuario();
                return ReturnData<List<ItemDecimalDto>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<ItemDto>>(ex);
            }
        } 
    } 
}
