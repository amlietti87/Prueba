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
using ROSBUS.Admin.Domain.Constants;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Admin.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _Configuration;
        private readonly ILogger logger;
        private IUserAppService _UserService;
        private ISysParametersService _parametersService;


        public AuthController(IUserAppService userService, IConfiguration configuration, ILogger _logger, ISysParametersService parametersService)
        {
            _UserService = userService;
            _Configuration = configuration;
            logger = _logger;
            _parametersService = parametersService;
        }


        [HttpGet("KeepAlive")]
        public IActionResult KeepAlive()
        {
            return Ok("live");
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> PostAsync([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                await logger.LogInformation("Modelo invalido");
                return BadRequest(ModelState);
            }

            try
            {
                var User = await _UserService.Login(credentials.Username, credentials.Password, credentials.CaptchaValue, true);

                var expirationtime = DateTime.Now.AddDays(1);
                var tokenGenerated = BuildToken(User, expirationtime);

                int sessionTimeout = 0;

                if (User.UserRoles.Count > 0)
                {
                    if (User.UserRoles.All(e => e.Role.CaducarSesionInactividad))
                    {
                        int.TryParse(_parametersService.GetAll(e => e.Token == SysParametersTokens.SESSION_TIMEOUT).Items.FirstOrDefault().Value, out sessionTimeout);
                    }
                }

                return Ok(new LoginOutput()
                {
                    username = credentials.Username,
                    email = User.Mail,
                    displayName = User.DisplayName.Trim(),
                    token = tokenGenerated,
                    expires = expirationtime,
                    sucursalId = User.SucursalId,
                    empleadoId = User.EmpleadoId,
                    sessionTimeout = sessionTimeout
                });
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { message= ex.Message, code= ex.HResult});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("intrabuslogin")]
        public async Task<IActionResult> IntrabusLogin([FromBody]CredentialsIntrabusModel accessToken)
        {
            if (!ModelState.IsValid)
            {
                await logger.LogInformation("Modelo invalido");
                return BadRequest(ModelState);
            }

            try
            {
                var intrabususer = await this._UserService.GetUserIntrabus(accessToken);

                var User = await _UserService.GetByIdAsync(intrabususer.FirstOrDefault().Id);

                var expirationtime = DateTime.Now.AddDays(1);
                var tokenGenerated = BuildToken(User, expirationtime);

                int sessionTimeout = 0;
                if (User.UserRoles.All(e => e.Role.CaducarSesionInactividad))
                {
                    int.TryParse(_parametersService.GetAll(e => e.Token == SysParametersTokens.SESSION_TIMEOUT).Items.FirstOrDefault().Value, out sessionTimeout);
                }


                return Ok(new LoginOutput()
                {
                    username = User.LogonName,
                    email = User.Mail,
                    displayName = User.DisplayName.Trim(),
                    token = tokenGenerated,
                    expires = expirationtime,
                    sucursalId = User.SucursalId,
                    empleadoId = User.EmpleadoId,
                    sessionTimeout = sessionTimeout
                });
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { message = ex.Message, code = ex.HResult });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/auth/login
        [HttpPost("loginMobile")]
        public async Task<IActionResult> LoginMobileAsync([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                await logger.LogInformation("Modelo invalido");
                return BadRequest(ModelState);
            }

            try
            {
                var User = await _UserService.Login(credentials.Username, credentials.Password, credentials.CaptchaValue);

                var expirationtime = DateTime.Now.AddDays(1);
                var tokenGenerated = BuildToken(User, expirationtime);

                return Ok(new LoginOutput()
                {
                    username = credentials.Username,
                    email = User.Mail,
                    displayName = User.DisplayName.Trim(),
                    token = tokenGenerated,
                    expires = expirationtime,
                    sucursalId = User.SucursalId,
                    empleadoId = User.EmpleadoId
                });
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { message = ex.Message, code = ex.HResult });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                await logger.LogInformation("Modelo invalido");
                return BadRequest(ModelState);
            }

            try
            {
                var resetPassword = await _UserService.ResetPassword(input);
                //UserName = user.UserName
                return Ok(resetPassword);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }


        private string BuildToken(SysUsersAd user, DateTime expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Username", user.LogonName),   
                new Claim("UserId", user.Id.ToString()),
                new Claim("SessionId", Guid.NewGuid().ToString())
                
            };

            var identityUrl = _Configuration.GetValue<string>("IdentityUrl");

            var token = new JwtSecurityToken(identityUrl, identityUrl,
              claims,
              expires: expiration,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}