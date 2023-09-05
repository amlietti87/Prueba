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

namespace ROSBUS.infra.Data.Repositories
{
    public class ReclamoCuotasRepository : RepositoryBase<AdminContext, SinReclamoCuotas, int>, IReclamoCuotasRepository
    {

        public ReclamoCuotasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinReclamoCuotas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<SinReclamoCuotas> AddAsync(SinReclamoCuotas entity)
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
    }
}
