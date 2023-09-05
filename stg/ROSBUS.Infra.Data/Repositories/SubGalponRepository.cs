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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class SubGalponRepository : RepositoryBase<AdminContext, SubGalpon, Decimal>, ISubGalponRepository
    {

        public SubGalponRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SubGalpon, bool>> GetFilterById(Decimal id)
        {
            return e => e.Id == id;
        }

        public override async Task<PagedResult<SubGalpon>> GetAllAsync(Expression<Func<SubGalpon, bool>> predicate, List<Expression<Func<SubGalpon, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SubGalpon> query = Context.Set<SubGalpon>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                return new PagedResult<SubGalpon>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<List<SubGalpon>> GetAllWithConfigu()
        {
            return await this.Context.SubGalpon.Where(e => e.FecBaja == null).Include(e => e.Configu).ToListAsync();
        }
        public async Task<SubGalpon> GetSubGalponWithLineaAndGalpon (decimal CodSubGalpon, decimal CodLin)
        {
           return await  this.Context.SubGalpon.Where(e => e.Configu.Any(f => f.CodSubg == CodSubGalpon && f.CodLin == CodLin)).FirstOrDefaultAsync();
        }
        protected override IQueryable<SubGalpon> AddIncludeForGet(DbSet<SubGalpon> dbSet)
        {


            return base.AddIncludeForGet(dbSet)
                    .Include(e => e.Configu)
                    .Include("Configu.Grupo")
                    .Include("Configu.Empresa")
                    .Include("Configu.Sucursal")
                    .Include("Configu.Linea")
                    .Include("Configu.Galpon")
                    .Include("Configu.PlanCamNav");
        }

    }
}
