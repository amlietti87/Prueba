using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IRoleService : IServiceBase<SysRoles, int>
    {
        Task<List<SysPermissions>> GetGrantedPermissionsAsync(int RolId);
        Task SetGrantedPermissionsAsync(int RolId, List<string> grantedPermissionNames);
    }
}
