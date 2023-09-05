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
    public class CroTipoElementoRepository : RepositoryBase<AdminContext,CroTipoElemento, int>, ICroTipoElementoRepository
    {

        public CroTipoElementoRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<CroTipoElemento, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<CroTipoElemento> AddIncludeForGet(DbSet<CroTipoElemento> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.CroElemeneto);
        }

        public async override Task<PagedResult<CroTipoElemento>> GetAllAsync(Expression<Func<CroTipoElemento, bool>> predicate, List<Expression<Func<CroTipoElemento, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<CroTipoElemento> query = Context.Set<CroTipoElemento>().Where(predicate).AsQueryable();
                
                var total = await query.CountAsync();
                return new PagedResult<CroTipoElemento>(total,  this.Context.CroTipoElemento.Include(e => e.CroElemeneto).ToList());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

    }
}
