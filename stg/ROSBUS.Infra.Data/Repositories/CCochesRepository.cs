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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.infra.Data.Operaciones.Repositories
{
    public class CCochesRepository : RepositoryBase<AdminContext, CCoches, string>, ICCochesRepository
    {

        public CCochesRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }


        protected override IQueryable<CCoches> AddIncludeForGet(DbSet<CCoches> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Empresa);
        }

        public override Expression<Func<CCoches, bool>> GetFilterById(string id)
        {
            return e => e.Id == id;
        }

        public override async Task<CCoches> AddAsync(CCoches entity)
        {

            return await base.AddAsync(entity);

        }
        public async Task<bool> ExisteCoche(string id)
        {
            var coches = await this.Context.CCoches.Where(e => e.Id == id).CountAsync();
            if (coches > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        public async Task<List<int>> GetLineaPorDefecto(int CodEmpleado, DateTime Fecha)
        {


            List<int> Linea = new List<int>();

            var sp = this.Context.LoadStoredProc("dbo.sp_GetLineaPorDefecto")

                .WithSqlParam("CodEmpleado", new SqlParameter("CodEmpleado", CodEmpleado))
                .WithSqlParam("Fecha", new SqlParameter("Fecha", Fecha ));



            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Linea = handler.ReadToList<int>().ToList();
            });

            return Linea;

        }

        public async Task<CCoches> GetCocheById(string id, DateTime FechaSiniestro)
        {

            CCoches Coche = new CCoches();
            var sp = this.Context.LoadStoredProc("dbo.sp_GetCochesById")

                .WithSqlParam("Id", new SqlParameter("Id", id))
                .WithSqlParam("FechaSiniestro", new SqlParameter("FechaSiniestro", FechaSiniestro));



            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Coche = handler.ReadToList<CCoches>().FirstOrDefault();
            });

            return Coche;
        }

        public async Task<List<CCoches>> GetAllCoches(DateTime FechaSiniestro, string Filter)
        {

            List<CCoches> Coche = new List<CCoches>();
            var sp = this.Context.LoadStoredProc("dbo.sp_GetAllCoches")

                .WithSqlParam("FechaSiniestro", new SqlParameter("FechaSiniestro", FechaSiniestro))
                            .WithSqlParam("Filter", new SqlParameter("Filter", Filter));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Coche = handler.ReadToList<CCoches>().ToList();
            });

            return Coche;
        }

        public override async Task<PagedResult<CCoches>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<CCoches> query = Context.Set<CCoches>().Where(filter.GetFilterExpression()).AsQueryable();

                var total = await query.CountAsync();

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }

                query = query.OrderBy(e => e.Interno);


                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                //var total = query.Count();


                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<CCoches>(total, await list.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        public async  Task<List<CCochesDto>> RecuperarCCochesPorFechaServicioLinea (DateTime Fecha, int? Cod_Servicio, int Cod_Linea)
        {
            List<CCochesDto> coches = new List<CCochesDto>();
            var sp = this.Context.LoadStoredProc("dbo.sp_RecuperarCCochesPorFechaServicioLinea")

                .WithSqlParam("Fecha", new SqlParameter("Fecha", Fecha))
                .WithSqlParam("Cod_Servicio", new SqlParameter("Cod_Servicio", (object)Cod_Servicio ?? DBNull.Value))
                .WithSqlParam("CodLin", new SqlParameter("CodLin", Cod_Linea));
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                coches = handler.ReadToList<CCochesDto>().ToList();
            });

            return coches;

        }

        public async Task<List<CCochesDto>> RecuperarCCoches (string FilterText)
        {
            List<CCochesDto> coches = new List<CCochesDto>();
            var sp = this.Context.LoadStoredProc("dbo.sp_RecuperarCCoches")
                .WithSqlParam("Filter", new SqlParameter("Filter", FilterText));
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                coches = handler.ReadToList<CCochesDto>().ToList();
            });

            return coches;
        }

    }
}
