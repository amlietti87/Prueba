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
    public class ConsecuenciasRepository : RepositoryBase<AdminContext,SinConsecuencias, int>, IConsecuenciasRepository
    {

        public ConsecuenciasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinConsecuencias, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<SinConsecuencias> AddIncludeForGet(DbSet<SinConsecuencias> dbSet)
        {


            return base.AddIncludeForGet(dbSet)
    .Include(e => e.SinCategorias);
        }

        public override async Task DeleteAsync(SinConsecuencias entity)
        {

            await Task.FromResult(Context.Set<SinConsecuencias>().Remove(entity));


            foreach (var TiempoEsperado in entity.SinCategorias.Reverse())
            {
                Context.Set<SinCategorias>().Remove(TiempoEsperado);

            }
        }

        public async override Task DeleteAsync(int id)
        {
            try
            {
                await DeleteAsync(await GetByIdAsync(id));
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        public override async Task<SinConsecuencias> AddAsync(SinConsecuencias entity)
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


        public async override Task<PagedResult<SinConsecuencias>> GetAllAsync(Expression<Func<SinConsecuencias, bool>> predicate, List<Expression<Func<SinConsecuencias, Object>>> includeExpression = null)
        {
            try
            {

                IQueryable<SinConsecuencias> query = Context.Set<SinConsecuencias>()
                            .Include("SinCategorias")
                            .Where(predicate).AsQueryable();

                var total = query.Count();
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<SinConsecuencias>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<List<SinConsecuencias>> GetConsecuenciasSinAnular()
        {
            var result = await this.Context.SinConsecuencias.Include(e => e.SinCategorias).Where(e => e.Anulado == false && e.IsDeleted == false).ToListAsync();

            return result;

        }
    }
}
