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

    public class RoleAppService : AppServiceBase<SysRoles, RoleDto, int, IRoleService>, IRoleAppService
    {
        protected readonly IPermissionService _PermissionService;
        public RoleAppService(IRoleService serviceBase, IPermissionService permissionService) 
            :base(serviceBase)
        {
            _PermissionService = permissionService;
        }
 

        public async  Task<GetPermissionsForEditOutput> GetRolePermissionsForEdit(int id)
        {
            var permissions = await this._PermissionService.GetAllAsync((a) => true);

            var grantedPermissions = await this._serviceBase.GetGrantedPermissionsAsync(id);

            GetPermissionsForEditOutput result = GetPermissionsForEditOutput.GetPermissionsForEdit(permissions.Items, grantedPermissions);

            return result;
        }

 
        public async Task UpdateRolePermissions(UpdateRolPermissionsInput input)
        {
            await _serviceBase.SetGrantedPermissionsAsync(input.Id, input.GrantedPermissionNames);
        }
    }
}
