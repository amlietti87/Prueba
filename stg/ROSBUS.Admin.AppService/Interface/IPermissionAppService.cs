using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IPermissionAppService : IAppServiceBase<SysPermissions, long>
    {
        Task<string[]> GetPermissionForCurrentUser();
        Task<string[]> GetPermissionForUser(int id);
    }
}
