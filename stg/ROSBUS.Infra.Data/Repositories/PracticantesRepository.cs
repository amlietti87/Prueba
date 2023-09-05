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
    public class PracticantesRepository : RepositoryBase<AdminContext, SinPracticantes, int>, IPracticantesRepository
    {

        public PracticantesRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinPracticantes, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        protected override IQueryable<SinPracticantes> AddIncludeForGet(DbSet<SinPracticantes> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
            .Include(e => e.TipoDoc);
        }

        public override async Task<SinPracticantes> AddAsync(SinPracticantes entity)
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

        public async override Task<PagedResult<SinPracticantes>> GetAllAsync(Expression<Func<SinPracticantes, bool>> predicate, List<Expression<Func<SinPracticantes, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SinPracticantes> query = Context.Set<SinPracticantes>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.ApellidoNombre);
                return new PagedResult<SinPracticantes>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }


    }
}
