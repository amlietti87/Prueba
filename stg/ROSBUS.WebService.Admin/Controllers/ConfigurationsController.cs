using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Url;

using TECSO.FWK.AppService.Interface;

namespace ROSBUS.WebService.Admin.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IAppUrlService appUrlService;

        public ConfigurationsController(IAppUrlService _appUrlService)
        {

            appUrlService = _appUrlService;
        }

        // POST api/auth/login
        [HttpGet]
        public JsonResult GetIconConfig()
        {
            return new JsonResult(appUrlService.GetAllIconMarkerUrl());
        }
        
    }
}