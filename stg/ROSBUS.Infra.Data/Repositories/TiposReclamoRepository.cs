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
using ROSBUS.ART.Domain.Interfaces.Repositories;
using ROSBUS.ART.Domain.Entities.ART;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class TiposReclamoRepository : RepositoryBase<AdminContext,TiposReclamo, int>, ITiposReclamoRepository
    {

        public TiposReclamoRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<TiposReclamo, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
      
        public override Task<TiposReclamo> UpdateAsync(TiposReclamo entity)
        {
            return base.UpdateAsync(entity);
        }

        public override Task<PagedResult<TiposReclamo>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            return base.GetPagedListAsync(filter);
        }

        public async override Task<PagedResult<TiposReclamo>> GetAllAsync(Expression<Func<TiposReclamo, bool>> predicate, List<Expression<Func<TiposReclamo, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<TiposReclamo> query = Context.Set<TiposReclamo>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<TiposReclamo>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
