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
    public class DshDashboardRepository : RepositoryBase<AdminContext,DshDashboard, int>, IDshDashboardRepository
    {

        public DshDashboardRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<DshDashboard, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<DshUsuarioDashboardItem>> RecuperarDshUsuarioDashboardItems(int userId)
        {
            var data = await this.Context.DshUsuarioDashboardItems.AsQueryable().Include(e => e.Dashboard).Where(e => e.CodUsuario == userId).ToListAsync();

            return data;
        }

        public async Task SaveDashboardUsuario(IEnumerable<DshUsuarioDashboardItem> items, int DashboardLayoutId)
        {

            foreach (var item in items)
            {
                item.CodUsuario = this.Context.GetAuditUserId().GetValueOrDefault();
            }
            var user = this.Context.SysUsersAd.FirstOrDefault(e=> e.Id==this.Context.GetAuditUserId());
            user.DashboardLayoutId = DashboardLayoutId;
            this.Context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            
            foreach (var item in items)
            {
                this.Context.Entry(item).State = item.Id <= 0 ? Microsoft.EntityFrameworkCore.EntityState.Added : Microsoft.EntityFrameworkCore.EntityState.Modified; 
            }


            foreach (var item in this.Context.DshUsuarioDashboardItems.Where(e=> e.CodUsuario== this.Context.GetAuditUserId().GetValueOrDefault()).ToList().Reverse<DshUsuarioDashboardItem>())
            {
                if (!items.Any(e=> e.DashboardId==item.DashboardId))
                {
                    this.Context.Entry(item).State = EntityState.Deleted;
                }
            }

            await this.Context.SaveChangesAsync();
        }
    }
}
