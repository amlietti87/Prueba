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
    public class SubEstadosRepository : RepositoryBase<AdminContext, SinSubEstados, int>, ISubEstadosRepository
    {

        public SubEstadosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinSubEstados, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<SinSubEstados> AddIncludeForGet(DbSet<SinSubEstados> dbSet)
        {


            return base.AddIncludeForGet(dbSet)
    .Include(e => e.Estado);
        }



        public override async Task<SinSubEstados> AddAsync(SinSubEstados entity)
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

        public override PagedResult<SinSubEstados> GetAll(Expression<Func<SinSubEstados, bool>> predicate)
        {
            return base.GetAll(predicate);
        }

        public async override Task<PagedResult<SinSubEstados>> GetAllAsync(Expression<Func<SinSubEstados, bool>> predicate, List<Expression<Func<SinSubEstados, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SinSubEstados> query = Context.Set<SinSubEstados>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                query = query.Include(e=> e.Estado);

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<SinSubEstados>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
