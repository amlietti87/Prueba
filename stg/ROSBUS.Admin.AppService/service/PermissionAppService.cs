using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class PermissionAppService : AppServiceBase<SysPermissions, long, IPermissionService>, IPermissionAppService
    {
        public PermissionAppService(IPermissionService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async Task<string[]> GetPermissionForCurrentUser()
        {
            return await this._serviceBase.GetPermissionForCurrentUser();
        }

        public async Task<string[]> GetPermissionForUser(int id)
        {
            return await this._serviceBase.GetPermissionForUser(id);
        }
    }
}
