using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class RoleRepository : RepositoryBase<AdminContext, SysRoles, int>, IRoleRepository
    {
        public RoleRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SysRoles, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<SysPermissions>> GetGrantedPermissionsAsync(int RoleId)
        {
            try
            {
                var result = await this.Context.SysPermissions.Where(e => e.PermissionRols.Any(a => a.RolId == RoleId)).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        } 
 

        public async Task SetGrantedPermissionsAsync(int RoleId, List<string> grantedPermissionNames)
        {
            try
            {
                var rp = await GetGrantedPermissionsAsync(RoleId);

                var GrantedPermissions = await this.Context.SysPermissions.Where(e => grantedPermissionNames.Contains(e.Token)).ToListAsync();

                var rolassign = rp.Except(GrantedPermissions);

                var assign = GrantedPermissions.Except(rp);


                foreach (var item in rolassign)
                {
                    var deleteitem = this.Context.SysPermissionsRoles.Where(e => e.RolId == RoleId && e.PermissionId == item.Id).FirstOrDefault();

                    if (deleteitem != null)
                    {
                        this.Context.Entry(deleteitem).State = EntityState.Deleted;
                    }

                }

                foreach (var item in assign)
                {
                    this.Context.SysPermissionsRoles.Add(new SysPermissionsRoles()
                    {
                        PermissionId = item.Id,
                        RolId = RoleId
                    });
                }


                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_sys_roles_name", "El codigo de rol ya existe");
            return d;
        }

    }
}
