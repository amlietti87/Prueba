using System;
using TECSO.FWK.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.Generic;
using ROSBUS.Admin.AppService.Interface;
using System.Linq;

namespace ROSBUS.WebService.Shared
{
    public class PermissionProvider : IPermissionProvider
    {
        
        private IMemoryCache _cache;
        protected IAuthService _authService { get; set; }

        private readonly IPermissionAppService permissionAppService;

        public PermissionProvider(IMemoryCache cache, IAuthService authService, IPermissionAppService _permissionAppService)
        {
            this._cache = cache;
            this._authService = authService;
            this.permissionAppService = _permissionAppService;
        }

        public async Task<string[]> GetPermissionForCurrentUser()
        {
            var sessionkey = _authService.GetSessionID();
            if (sessionkey != null)
            {
                var p = _cache.Get<List<string>>("GetPermissionForCurrentUser_" + sessionkey);

                if (p == null)
                {
                    p = (await permissionAppService.GetPermissionForCurrentUser()).ToList();
                    _cache.Set("GetPermissionForCurrentUser_" + sessionkey, p);
                }

                return p.ToArray();
            }
            return new string[0];

        }



    }
}
