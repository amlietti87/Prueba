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
using ROSBUS.Admin.Domain.Entities.Filters;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class AdjuntosRepository : RepositoryBase<AdjuntosContext,Adjuntos, Guid>, IAdjuntosRepository
    {

        public AdjuntosRepository(IAdjuntosDbContext _context)
            :base(new DbContextProvider<AdjuntosContext>(_context))
        {

        }

        public override Expression<Func<Adjuntos, bool>> GetFilterById(Guid id)
        {
            return e => e.Id == id;
        }


        public override async Task<Adjuntos> AddAsync(Adjuntos entity)
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

        public async Task<List<ItemDto<Guid>>> GetAdjuntosItemDto(AdjuntosFilter filter)
        {
            var adj = await this.Context.Adjuntos.Where(e => filter.Ids.Any(id => id == e.Id)).Select(e => new ItemDto<Guid>() { Id = e.Id, Description = e.Nombre }).ToListAsync();

            return adj;
        }
        public async Task<ItemDto<Guid>> GetAdjuntoItemDto(Guid Id)
        {
            var adj = await this.Context.Adjuntos.Where(e => e.Id == Id).Select(e => new ItemDto<Guid>() { Id = e.Id, Description = e.Nombre }).FirstOrDefaultAsync();

            return adj;
        }
    }
}
