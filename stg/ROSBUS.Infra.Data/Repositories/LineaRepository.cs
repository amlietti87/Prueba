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
using TECSO.FWK.Domain.Auditing;
using System.Data.SqlClient;
using TECSO.FWK.Domain;

namespace ROSBUS.infra.Data.Repositories
{
    public class LineaRepository : RepositoryBase<AdminContext,Linea, decimal>, ILineaRepository
    {

        public LineaRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<Linea, bool>> GetFilterById(decimal id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<Linea> AddIncludeForGet(DbSet<Linea> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.SucursalesxLineas);
        }

        public override async Task DeleteAsync(Linea entity)
        {
            entity.FecBaja = DateTime.Now;
            await base.DeleteAsync(entity);
          
            foreach (var ramal in Context.PlaRamalColor.Where(e => e.LineaId == entity.Id).ToList())
            {
                Context.Set<PlaRamalColor>().Remove(ramal);
                foreach (var bandera in Context.HBanderas.Include(e => e.Rutas).Where(e => e.RamalColorId == ramal.Id).ToList())
                {
                    Context.Set<HBanderas>().Remove(bandera);
                    foreach (var Rutas in bandera.Rutas)
                    {
                        Context.Set<GpsRecorridos>().Remove(Rutas);
                    }

                }

            }


        }
        
        public async override Task<Linea> AddAsync(Linea entity)
        {
            try
            {
                var max = Context.Linea.Max(e => e.Id);
                entity.Id = max + 1;

                DbSet<Linea> dbSet = Context.Set<Linea>();
                var entry = await dbSet.AddAsync(entity);
                await this.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("50010"))
                {
                    throw new DomainValidationException("Recuerde que el codigo a insertar sea el mismo para san_martin.general, ba - blade - 01.general y operacionesRB");
                }
                else {
                    this.HandleException(ex);
                    throw;
                }                
            }
        }


        public override Task<Linea> GetByIdAsync<TFilter>(TFilter filter)
        {
            return base.GetByIdAsync(filter);
        }
    }
}
