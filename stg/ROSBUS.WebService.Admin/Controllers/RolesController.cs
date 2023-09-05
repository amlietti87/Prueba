using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class RolesController : ManagerSecurityController<SysRoles, int, RoleDto, RoleFilter, IRoleAppService>
    {
        public RolesController(IRoleAppService service)
            : base(service)
        {

            //Admin.User.Permisos

        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "Rol");
            this.PermissionContainer.AddPermission("UpdateRolePermissions", "Admin", "Rol", "Permisos");
        }

        [HttpGet]
        public async Task<IActionResult> GetRolePermissionsForEdit(int id)
        {
            try
            {
                return ReturnData(await (this.Service as IRoleAppService).GetRolePermissionsForEdit(id));
            }
            catch (Exception ex)
            {
                return ReturnError<GetPermissionsForEditOutput>(ex);
            }
        }

        [HttpPost]
        [ActionAuthorize()]
        public async Task<IActionResult> UpdateRolePermissions([FromBody] UpdateRolPermissionsInput input)
        {
            try
            {
                await (this.Service as IRoleAppService).UpdateRolePermissions(input);
                return ReturnData<string>(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }




    }


 

}
