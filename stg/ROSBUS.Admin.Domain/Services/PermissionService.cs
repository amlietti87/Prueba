using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PermissionService : ServiceBase<SysPermissions, long, IPermissionRepository>, IPermissionService
    {       
        public PermissionService(IPermissionRepository repository)
            : base(repository)
        {
            
        }

        public async Task<string[]> GetPermissionForCurrentUser()
        {
            return await this.repository.GetPermissionForCurrentUser();
        }

        public async Task<string[]> GetPermissionForUser(int id)
        {
            return await this.repository.GetPermissionForUser(id);
        }
    }
    
}
