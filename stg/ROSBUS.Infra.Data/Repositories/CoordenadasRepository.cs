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
using ROSBUS.Admin.Domain.Entities.Filters;
using Snickler.EFCore;
using System.Data.SqlClient;

namespace ROSBUS.infra.Data.Repositories
{
    public class CoordenadasRepository : RepositoryBase<AdminContext,PlaCoordenadas, int>, ICoordenadasRepository
    {

        public CoordenadasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaCoordenadas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<PlaCoordenadas> AddAsync(PlaCoordenadas entity)
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



        public override Task<PagedResult<PlaCoordenadas>> GetAllAsync(Expression<Func<PlaCoordenadas, bool>> predicate, List<Expression<Func<PlaCoordenadas, object>>> includeExpression = null)
        {
            return base.GetAllAsync(predicate, includeExpression);
        }


        public async Task<List<PlaCoordenadas>> RecuperarCoordenadasPorFecha(CoordenadasFilter Filtro)
        {
            List<PlaCoordenadas> itemDtos = new List<PlaCoordenadas>();

            var sp = this.Context.LoadStoredProc("dbo.Sp_Pla_LeerCoordenadasPorFecha")

                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha))
                .WithSqlParam("Abreviacion", new SqlParameter("Abreviacion", (Object)Filtro.Abreviacion ?? DBNull.Value));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<PlaCoordenadas>().ToList();
            });

            return itemDtos;
        }



    }
}
