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
    public class EstadosRepository : RepositoryBase<AdminContext, SinEstados, int>, IEstadosRepository
    {

        public EstadosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinEstados, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<SinEstados> AddAsync(SinEstados entity)
        {
            //try
            //{
            //    DbSet<PlaCoordenadas> dbSet = Context.Set<PlaCoordenadas>();
            //    EntityEntry<PlaCoordenadas> entittyentry;

            //    if (!dbSet.Any(e => e.Id == entity.Id)) {
            //        entittyentry = await dbSet.AddAsync(entity);
            //    }
            //    else{

            //        entittyentry = dbSet.Attach(entity);
            //        entittyentry.State = EntityState.Modified;
            //    }

            //    await this.SaveChangesAsync();
            //    return entittyentry.Entity;
            //}
            //catch (Exception ex)
            //{
            //    this.HandleException(ex);
            //    throw;
            //}

            return await base.AddAsync(entity);

        }

        protected override IQueryable<SinEstados> AddIncludeForGet(DbSet<SinEstados> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.SinSubEstados);
        }

        public async override Task<PagedResult<SinEstados>> GetAllAsync(Expression<Func<SinEstados, bool>> predicate, List<Expression<Func<SinEstados, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SinEstados> query = Context.Set<SinEstados>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<SinEstados>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
