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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class PlaTalleresIvuRepository : RepositoryBase<AdminContext, PlaTalleresIvu, int>, IPlaTalleresIvuRepository
    {

        public PlaTalleresIvuRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        protected override IQueryable<PlaTalleresIvu> AddIncludeForGet(DbSet<PlaTalleresIvu> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.CodGalNavigation);
        }

        protected override IQueryable<PlaTalleresIvu> GetIncludesForPageList(IQueryable<PlaTalleresIvu> query)
        {
            return base.GetIncludesForPageList(query).Include(e => e.CodGalNavigation);
        }

        public override Expression<Func<PlaTalleresIvu, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public async Task<Galpon> GetGalponWithIvu (int CodUbicacion)
        {
            var galpon = await this.Context.PlaTalleresIvu.Where(e => e.CodGalIvu == CodUbicacion).Include(f=> f.CodGalNavigation).FirstOrDefaultAsync();
            return galpon?.CodGalNavigation;
        }

        public override async Task<PlaTalleresIvu> AddAsync(PlaTalleresIvu entity)
        {


            return await base.AddAsync(entity);

        }


        public async override Task<PagedResult<PlaTalleresIvu>> GetAllAsync(Expression<Func<PlaTalleresIvu, bool>> predicate, List<Expression<Func<PlaTalleresIvu, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<PlaTalleresIvu> query = Context.Set<PlaTalleresIvu>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                return new PagedResult<PlaTalleresIvu>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

    }
}
