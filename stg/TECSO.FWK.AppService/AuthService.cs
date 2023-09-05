using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace TECSO.FWK.AppService
{
    public class AuthService : IAuthService
    {
        IHttpContextAccessor _httpContextAccessor;

 

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurretUserId()
        {
            var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "UserId")?.Value;
            if (!String.IsNullOrEmpty(value))
            {
                return int.Parse(value);
            }
            return default(int);
        }

        public string GetCurretToken()
        {
            var auth = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"];

            if (auth.HasValue)
            {
                var access_token = auth.GetValueOrDefault().FirstOrDefault() ?? "";
                access_token = access_token.Replace("Bearer ", "");
                return access_token;
            }

            return null;
        }

        public string GetSessionID()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "SessionId")?.Value;
        }

        public string GetCurretUserName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "Username")?.Value;
        } 
    }
}
