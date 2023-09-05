using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class RoleService : ServiceBase<SysRoles,int, IRoleRepository>, IRoleService
    {
         
        public RoleService(IRoleRepository repository)
            : base(repository)
        {
            
        }

        public async Task<List<SysPermissions>> GetGrantedPermissionsAsync(int rolId)
        {
            return await repository.GetGrantedPermissionsAsync(rolId);
        }

        public async Task SetGrantedPermissionsAsync(int rolId, List<string> grantedPermissionNames)
        {
            await repository.SetGrantedPermissionsAsync(rolId, grantedPermissionNames);
        }


        public override async Task DeleteAsync(int id)
        {
            var role = await this.repository.GetByIdAsync(id);
            if (role.IsStatic)
            {
                throw new DomainValidationException("No se puede eliminar un Rol de sistema");
            }
            await base.DeleteAsync(id);
        }



    }
    
}
