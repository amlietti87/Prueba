using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;

using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    // [Authorize]
    public class SearchController : TECSO.FWK.ApiServices.ControllerBase
    {
        private ILineaAppService Service;

        public SearchController(ILineaAppService _Service)
        {
            this.Service = _Service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string FilterText)
        {
            try
            {
                SearchResultDto result = await Service.Search(FilterText);
                 
                return ReturnData(result);
            }
            catch (Exception ex)
            {
                return ReturnError<AppMenu>(ex);
            }
        }

    }

 
}