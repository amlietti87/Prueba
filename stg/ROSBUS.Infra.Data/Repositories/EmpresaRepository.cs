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
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class EmpresaRepository : RepositoryBase<AdminContext,Empresa, Decimal>, IEmpresaRepository
    {

        public EmpresaRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<Empresa, bool>> GetFilterById(Decimal id)
        {
            return e => e.Id == id;
        }

        public async override Task<PagedResult<Empresa>> GetAllAsync(Expression<Func<Empresa, bool>> predicate, List<Expression<Func<Empresa, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<Empresa> query = Context.Set<Empresa>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.DesEmpr);
                return new PagedResult<Empresa>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }


    }
}
