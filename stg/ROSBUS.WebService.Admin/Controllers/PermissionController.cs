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
    [Route("api/[controller]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    [Authorize]
    public class PermissionController : TECSO.FWK.ApiServices.ControllerBase
    {
        private IPermissionAppService Service;

        public PermissionController(IPermissionAppService _Service)
        {
            this.Service = _Service;
        }
        
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                String[] entity = await this.Service.GetPermissionForCurrentUser();

                return ReturnData(entity);
            }
            catch (Exception ex)
            {
                return ReturnError<UserDto>(ex);
            }
        }

    }

 
}