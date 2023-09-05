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
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.WebService.Mobile.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAppService _service;

        public AuthController(IAuthAppService service)
        {
            this._service = service;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> PostAsync([FromBody]CredentialsViewModel credentials)
        {
            try
            {
                var User = await _service.Login(credentials.Username, credentials.Password, credentials.CaptchaValue);                                
                return Ok(User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }
    }
}