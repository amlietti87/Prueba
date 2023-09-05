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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Data.SqlClient;
using GeoCoordinatePortable;
using Snickler.EFCore;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class PuntosRepository : RepositoryBase<AdminContext,PlaPuntos, Guid>, IPuntosRepository
    {

        public PuntosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaPuntos, bool>> GetFilterById(Guid id)
        {
            return e => e.Id == id;
        }

        public async Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf)
        {

            List<PlaPuntos> result = new List<PlaPuntos>();

            var sp = this.Context.LoadStoredProc("dbo.sp_pla_Puntos_GetFilterPuntosInicioFin")

                .WithSqlParam("cod_sucursal", new SqlParameter("cod_sucursal", ((object)pf.UnidadDeNegocioId ?? DBNull.Value)))
                ;



            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result = handler.ReadToList<PlaPuntos>().ToList();
            });

            //var result =  await this.Context.Set<PlaPuntos>()
            //    .FromSql("exec sp_pla_Puntos_GetFilterPuntosInicioFin @cod_sucursal ", new SqlParameter("cod_sucursal", ((object)pf.UnidadDeNegocioId ?? DBNull.Value))).ToListAsync();

            return result;
        }


        public override PagedResult<PlaPuntos> GetAll(Expression<Func<PlaPuntos, bool>> predicate)
        {
            try
            {
                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<PlaPuntos> query = Context.Set<PlaPuntos>().Where(predicate).AsQueryable();

                var total = query.Count();

                query = query.Include(e => e.PlaCoordenada);

                return new PagedResult<PlaPuntos>(total, query.ToList());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public override async Task<PagedResult<PlaPuntos>> GetAllAsync(Expression<Func<PlaPuntos, bool>> predicate, List<Expression<Func<PlaPuntos, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<PlaPuntos> query = Context.Set<PlaPuntos>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.PlaCoordenada).Include(e=> e.PlaParada);

                return new PagedResult<PlaPuntos>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
        public async Task<List<GpsDetaReco>> RecuperarDatosIniciales(int CodRec)
        {
            var query = this.Context.GpsDetaReco.Where(e => e.CodRec == CodRec  );

            if (!!query.Any())
            {
                var max = query.Max(e => e.Cuenta);
                var min = query.Min(e => e.Cuenta);

                var result = await this.Context.GpsDetaReco.Where(e =>
                 e.CodRec == CodRec &&
                 (e.Sector == "1" || e.Cuenta == max || e.Cuenta == min)
                ).ToListAsync();

                return result;
            }

            return new List<GpsDetaReco>();

        }

        public async Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec)
        {

            List<GpsDetaReco> result = new List<GpsDetaReco>();

            var query = this.Context.GpsDetaReco.Where(e => e.CodRec == CodRec);

            var count = await query.CountAsync();

            result = await query.ToListAsync();

            return result;



        }


    }
}
