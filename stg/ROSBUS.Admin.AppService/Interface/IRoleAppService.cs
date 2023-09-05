using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IRoleAppService : IAppServiceBase<SysRoles, RoleDto, int>
    {
        Task UpdateRolePermissions(UpdateRolPermissionsInput input);
        Task<GetPermissionsForEditOutput> GetRolePermissionsForEdit(int id);
    }
}
