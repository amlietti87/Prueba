using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdAccionesPermitidasRepository : RepositoryBase<AdminContext, FdAccionesPermitidas, int>, IFdAccionesPermitidasRepository
    {

        public FdAccionesPermitidasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<FdAccionesPermitidas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<FdAccionesPermitidas> AddIncludeForGet(DbSet<FdAccionesPermitidas> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e=> e.Permiso);
        }


        public async override Task<PagedResult<FdAccionesPermitidas>> GetAllAsync(Expression<Func<FdAccionesPermitidas, bool>> predicate, List<Expression<Func<FdAccionesPermitidas, Object>>> includeExpression = null)
        {
            try
            {

                IQueryable<FdAccionesPermitidas> query = Context.Set<FdAccionesPermitidas>()
                            .Include(e=> e.Permiso)
                            .Where(predicate).AsQueryable();

                var total = query.Count();

                return new PagedResult<FdAccionesPermitidas>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
