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
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.infra.Data.Repositories
{
    public class UserRepository : RepositoryBase<AdminContext, SysUsersAd, int>, IUserRepository
    {

        public UserRepository(AdminContext _context, IAdminDbContext a)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            var s = _context.GetHashCode() == a.GetHashCode();
        }

        public override Expression<Func<SysUsersAd, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<SysUsersAd> GetUser(string Username)
        {
            return await this.Context.SysUsersAd
                .Include(e => e.UserRoles)
                .ThenInclude(f=> f.Role)
                .FirstOrDefaultAsync(e => !e.IsDeleted && e.LogonName == Username);
        }

        public async Task<List<SysUsersRoles>> GetUserRoles(int UserId)
        {
            return await this.Context.SysUsersRoles.Where(e => e.UserId == UserId).ToListAsync();
        }

        protected override IQueryable<SysUsersAd> AddIncludeForGet(DbSet<SysUsersAd> dbSet)
        {
            var query = base.AddIncludeForGet(dbSet).Include("UserRoles.Role");
            return query;
        }

        public override Task<SysUsersAd> UpdateAsync(SysUsersAd entity)
        {

            return base.UpdateAsync(entity);
        }

        public override async Task<SysUsersAd> AddAsync(SysUsersAd entity)
        {
            using (var ts = await this.Context.Database.BeginTransactionAsync())
            {
                entity.Id = this.Context.SysUsersAd.Max(e => e.Id) + 1;
                var r = await base.AddAsync(entity);
                ts.Commit();
                return r;
            }

        }


        public async Task<List<SysPermissions>> GetGrantedPermissionsAsync(int UserId)
        {
            try
            {
                // var result = await this.Context.Permissions.Join(this.Context.PermissionsUsers,
                //       p => p.Id,
                //   u => u.PermissionId,
                //   (per, userper) => new { u = userper, p = per }) // selection
                //.Where(w => w.u.UserId == UserId).Select(s => s.p).ToListAsync();
                // return result;
                var result = await this.Context.SysPermissions.Where(e => e.PermissionsUsers.Any(a => a.UserId == UserId)).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
       
        }

        public async Task SetGrantedPermissionsAsync(int userId, List<string> grantedPermissionNames)
        {
            try
            {
                var up = await GetGrantedPermissionsAsync(userId);
                
                var GrantedPermissions = await this.Context.SysPermissions.Where(e => grantedPermissionNames.Contains(e.Token)).ToListAsync();

                var unassign = up.Except(GrantedPermissions);

                var assign = GrantedPermissions.Except(up);


                foreach (var item in unassign)
                {
                    var deleteitem = this.Context.SysPermissionsUsers.Where(e => e.UserId == userId && e.PermissionId == item.Id).FirstOrDefault();

                    if (deleteitem != null)
                    {
                        this.Context.Entry(deleteitem).State = EntityState.Deleted;
                    }                  

                }

                foreach (var item in assign)
                {
                    this.Context.SysPermissionsUsers.Add(new SysPermissionsUsers()
                    {
                        PermissionId = item.Id,
                        UserId = userId
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

        public async Task<List<PlaLineasUsuarios>> GetUserLineasForEdit(int userId)
        {
            try
            {                
                var result = await this.Context.PlaLineasUsuarios.Include(e =>  e.CodLinNavigation).Where(e => e.UserId == userId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task SetUserLineasForEditAsync(int userId, List<ItemDecimalDto> lineas)
        {
            try
            {
                var up = await GetUserLineasForEdit(userId);


                var unassign = up.Where(e => !lineas.Any(l => l.Id == e.CodLin));

                var assign = lineas.Where(l => !up.Any(e => e.CodLin == l.Id));


                foreach (var item in unassign)
                {
                    var deleteitem = this.Context.PlaLineasUsuarios.Where(e => e.UserId == userId && e.CodLin == item.CodLin).FirstOrDefault();

                    if (deleteitem != null)
                    {
                        this.Context.Entry(deleteitem).State = EntityState.Deleted;
                    }

                }

                foreach (var item in assign)
                {
                    this.Context.PlaLineasUsuarios.Add(new PlaLineasUsuarios()
                    {
                        CodLin = item.Id,
                        UserId = userId
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

        public async Task<bool> TieneDiagramaActivo(int userId)
        {
            Boolean? isValid = true;

            var sp = this.Context.LoadStoredProc("dbo.sp_SysUsersAd_TieneDiagramaActivo")

                .WithSqlParam("userId", new SqlParameter("userId", userId));
                

            

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                isValid = handler.ReadToValue<Boolean>();

            });

            return isValid.GetValueOrDefault();
        }

        public async Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken)
        {
            List<ItemDto> user = new List<ItemDto>();
            var sp = this.Context.LoadStoredProc("dbo.sp_sys_UserAD_IntrabusLogin")
                    .WithSqlParam("userId", new SqlParameter("accesToken", accessToken.AccesToken));


            await sp.ExecuteStoredProcAsync((handler) =>
            {
                var res = handler.ReadToList<ItemDto>();
                user = res.ToList();

            });

            return user;
        }
    }
}
