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
    public class ReclamosHistoricosRepository : RepositoryBase<AdminContext,SinReclamosHistoricos, int>, IReclamosHistoricosRepository
    {

        public ReclamosHistoricosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinReclamosHistoricos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<SinReclamosHistoricos> AddAsync(SinReclamosHistoricos entity)
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


        protected override IQueryable<SinReclamosHistoricos> AddIncludeForGet(DbSet<SinReclamosHistoricos> dbSet)
        {

            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Abogado)
                .Include(e => e.AbogadoEmpresa)
                .Include(e => e.Estado)
                .Include(e => e.Involucrado)
                .Include(e => e.Involucrado.TipoDoc)
                .Include(e => e.Involucrado.TipoInvolucrado)
                .Include(e => e.Juzgado)
                .Include(e => e.SubEstado)
                .Include(e => e.TipoReclamo)
                .Include(e => e.Siniestro)
                .Include("Siniestro.Sucursal")
                .Include(e => e.CreatedUser)
                .Include(e => e.Denuncia)
                .Include(e => e.Denuncia.PrestadorMedico)
                ;
        }

        public override async Task<PagedResult<SinReclamosHistoricos>> GetAllAsync(Expression<Func<SinReclamosHistoricos, bool>> predicate, List<Expression<Func<SinReclamosHistoricos, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SinReclamosHistoricos> query = Context.Set<SinReclamosHistoricos>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }

                return new PagedResult<SinReclamosHistoricos>(total, await query.OrderByDescending(e => e.Fecha).ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
