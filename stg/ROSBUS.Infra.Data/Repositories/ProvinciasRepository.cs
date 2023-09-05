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
    public class ProvinciasRepository : RepositoryBase<OperacionesRBContext, Provincias, int>, IProvinciasRepository
    {

        public ProvinciasRepository(IOperacionesRBDbContext _context)
            :base(new DbContextProvider<OperacionesRBContext>(_context))
        {

        }

        public override Expression<Func<Provincias, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<Provincias> AddAsync(Provincias entity)
        {
            using (var ts = await this.Context.Database.BeginTransactionAsync())
            {
                entity.Id = this.Context.Provincias.Max(e => e.Id) + 1;
                var r = await base.AddAsync(entity);
                ts.Commit();
                return r;

            }
        }

        public override async Task<PagedResult<Provincias>> GetAllAsync(Expression<Func<Provincias, bool>> predicate, List<Expression<Func<Provincias, object>>> includeExpression = null)
        {
            try
            {
                var hola = Context.Set<Provincias>();
                IQueryable<Provincias> query = hola.Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }

                return new PagedResult<Provincias>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
