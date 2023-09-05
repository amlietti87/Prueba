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
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Interfaces.Repositories;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class TiposAcuerdoRepository : RepositoryBase<AdminContext,TiposAcuerdo, int>, ITiposAcuerdoRepository
    {

        public TiposAcuerdoRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<TiposAcuerdo, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async override Task<PagedResult<TiposAcuerdo>> GetAllAsync(Expression<Func<TiposAcuerdo, bool>> predicate, List<Expression<Func<TiposAcuerdo, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<TiposAcuerdo> query = Context.Set<TiposAcuerdo>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<TiposAcuerdo>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
