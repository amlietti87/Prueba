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
    public class BolBanderasCartelRepository : RepositoryBase<AdminContext,BolBanderasCartel, int>, IBolBanderasCartelRepository
    {

        public BolBanderasCartelRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<BolBanderasCartel, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<PagedResult<BolBanderasCartel>> GetAllAsync(Expression<Func<BolBanderasCartel, bool>> predicate, List<Expression<Func<BolBanderasCartel, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<BolBanderasCartel> query = Context.Set<BolBanderasCartel>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        var includeString = new FixVisitor(include).GetInclude();
                        
                        query = query.Include(includeString);                        
                    }

                }

                query = query.Include(e => e.BolBanderasCartelDetalle).ThenInclude(e => e.CodBanNavigation);

                return new PagedResult<BolBanderasCartel>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
