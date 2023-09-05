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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class RamalColorRepository : RepositoryBase<AdminContext, PlaRamalColor, Int64>, IRamalColorRepository
    {


        public RamalColorRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaRamalColor, bool>> GetFilterById(Int64 id)
        {
            return e => e.Id == id;
        }


        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_RamalSube", "Existen empresas duplicadas");
            return d;
        }


        protected override IQueryable<PlaRamalColor> AddIncludeForGet(DbSet<PlaRamalColor> dbSet)
        {
            var query = base.AddIncludeForGet(dbSet);

            //query = query.Include("Linea.UnidadDeNegocio").Include("RamalSube").Include("RamalSube.Empresa");

            query = query.Include(e => e.PlaLinea.Sucursal);

            return query;
        }


        public override async Task DeleteAsync(PlaRamalColor entity)
        {
            await base.DeleteAsync(entity);


            foreach (var bandera in Context.HBanderas.Include(e => e.Rutas).Where(e => e.RamalColorId == entity.Id).ToList())
            {
                Context.Set<HBanderas>().Remove(bandera);
                foreach (var Rutas in bandera.Rutas)
                {
                    Context.Set<GpsRecorridos>().Remove(Rutas);
                }
            }
        }


        public override async Task<PagedResult<PlaRamalColor>> GetAllAsync(Expression<Func<PlaRamalColor, bool>> predicate, List<Expression<Func<PlaRamalColor, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<PlaRamalColor> query = (from rc in Context.Set<PlaRamalColor>()
                                                   join hb in Context.Set<HBanderas>() on rc.Id equals hb.RamalColorId
                                                   join sb in Context.Set<PlaSentidoBandera>() on hb.SentidoBanderaId equals sb.Id
                                                   select new PlaRamalColor
                                                   {
                                                       Id = rc.Id,
                                                       Nombre = rc.Nombre + " (" + sb.Descripcion + ")",
                                                       LineaId = rc.LineaId,
                                                       Activo = rc.Activo
                                                   }).Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                return new PagedResult<PlaRamalColor>(total, await query.ToListAsync());

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public async Task<PagedResult<PlaRamalColor>> GetAllSinSentidos(Expression<Func<PlaRamalColor, bool>> predicate, List<Expression<Func<PlaRamalColor, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<PlaRamalColor> query = Context.Set<PlaRamalColor>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }

                return new PagedResult<PlaRamalColor>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

    }
}
