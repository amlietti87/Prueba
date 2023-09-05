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
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class PermissionsRepository : RepositoryBase<AdminContext, SysPermissions, long>, IPermissionRepository
    {
        public PermissionsRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SysPermissions, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        public async override Task<PagedResult<SysPermissions>> GetAllAsync(Expression<Func<SysPermissions, bool>> predicate, List<Expression<Func<SysPermissions, Object>>> includeExpression = null)
        {
            try
            {

                IQueryable<SysPermissions> query = Context.Set<SysPermissions>()
                            .Include("Pages")
                            .Include("Areas")
                            .Where(predicate).AsQueryable();

                var total = query.Count();

                return new PagedResult<SysPermissions>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<string[]> GetPermissionForCurrentUser()
        {
            try
            {
                var UserID = Context.GetAuditUserId();

                return await this.GetPermissionForUser(UserID.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<string[]> GetPermissionForUser(int id)
        {
            try
            {

                var ur = Context.SysUsersRoles.Where(u => u.UserId == id && !u.Role.IsDeleted).Select(e => e.RoleId).ToList();

                var result = await this.Context.SysPermissions.Where(
                    e => e.PermissionsUsers.Any(a => a.UserId == id)
                    || e.PermissionRols.Any(r => ur.Contains(r.RolId) && !r.Rol.IsDeleted)
                    ).Select(s => s.Token).ToListAsync();

                return result.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
