using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using Snickler.EFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class SiniestrosRepository : RepositoryBase<AdminContext, SinSiniestros, int>, ISiniestrosRepository
    {

        public SiniestrosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinSiniestros, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<SinSiniestros> AddIncludeForGet(DbSet<SinSiniestros> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.CreatedUser)
                .Include(e => e.Sucursal)
                .Include(e => e.Empresa)
                .Include(e => e.ConductorEmpresa)
                .Include(e => e.CocheLinea)
                .Include(e => e.Practicante)
                .Include("Practicante.TipoDoc")
                //.Include(e => e.SinInvolucrados).ThenInclude(a=> a.SinDetalleLesion)
                .Include(e => e.SinSiniestrosConsecuencias)
                .Include(e => e.SubCausa)
                .Include(e => e.SancionSugerida)
                .Include("SinSiniestrosConsecuencias.Consecuencia")
                .Include("SinSiniestrosConsecuencias.Categoria")
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.Lesionado)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.MuebleInmueble)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.Lesionado.TipoLesionado)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.MuebleInmueble.TipoInmueble)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.TipoDoc)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.TipoInvolucrado)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.Vehiculo)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.Conductor)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.SinDetalleLesion)
                //.Include(e => e.SinInvolucrados).ThenInclude(e => e.Conductor.TipoDoc)
                ;

        }

        public async Task<List<Licencias>> GetLicencias(int cod_empleado)
        {

            List<Licencias> Licencias = new List<Licencias>();
            var sp = this.Context.LoadStoredProc("dbo.GetLicenciasEmpleado")
                .WithSqlParam("cod_empleado", new SqlParameter("cod_empleado", cod_empleado));



            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Licencias = handler.ReadToList<Licencias>().ToList();
            });


            return Licencias;
        }
        public override async Task<SinSiniestros> AddAsync(SinSiniestros entity)
        {

            try
            {

                var result = await base.AddAsync(entity);
                return result;


            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }



        }

        protected override IQueryable<SinSiniestros> GetIncludesForPageList(IQueryable<SinSiniestros> query)
        {
            return base.GetIncludesForPageList(query).Include(e => e.Sucursal).Include(e=> e.SinSiniestrosConsecuencias).Include("SinSiniestrosConsecuencias.Consecuencia");
        }
        public override async Task<PagedResult<SinSiniestros>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<SinSiniestros> query = Context.Set<SinSiniestros>().Where(filter.GetFilterExpression()).AsQueryable();

                //var total = await query.CountAsync();

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }
                query = query.OrderByDescending(e => e.Fecha).ThenBy(f => f.Hora);
                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                var total = await query.CountAsync();


                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);
                var lista = await list.ToListAsync();
                return new PagedResult<SinSiniestros>(total, lista);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public override async Task<PagedResult<SinSiniestros>> GetAllAsync(Expression<Func<SinSiniestros, bool>> predicate, List<Expression<Func<SinSiniestros, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<SinSiniestros> query = Context.Set<SinSiniestros>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                return new PagedResult<SinSiniestros>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public async Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id, string dni)
        {
            HistorialSiniestros hist = new HistorialSiniestros();
            if (empleado)
            {
                int TotalEmpleado = await this.Context.SinSiniestros.Where(e => e.ConductorId == id).CountAsync();

                var IdPracticante = await this.Context.SinPracticantes.Where(e => e.NroDoc.Trim().ToLower() == dni.Trim().ToLower()).Select(e => e.Id).FirstOrDefaultAsync();
                int TotalPracticante = 0;
                int UltimoAnio = 0;
                if (IdPracticante != 0)
                {
                    TotalPracticante = await this.Context.SinSiniestros.Where(e => e.PracticanteId == IdPracticante).CountAsync();
                    UltimoAnio = await this.Context.SinSiniestros.Where(e => (e.ConductorId == id || e.PracticanteId == IdPracticante) && e.Fecha.Year >= DateTime.Now.Year).CountAsync();
                }
                else
                {
                    UltimoAnio = await this.Context.SinSiniestros.Where(e => e.ConductorId == id && e.Fecha.Year >= DateTime.Now.Year).CountAsync();
                }

                hist.Practicante = TotalPracticante;
                hist.Total = TotalEmpleado + TotalPracticante;
                hist.UltimoAnio = UltimoAnio;

            }
            else
            {
                hist.Practicante = await this.Context.SinSiniestros.Where(e => e.PracticanteId == id).CountAsync();
            }

            return hist;
        }


        public async Task<int> GetNroSiniestroMax()
        {
            var nrosiniestromax = await Context.SinSiniestros.MaxAsync(e => Convert.ToInt32(e.NroSiniestro));
            return nrosiniestromax;
        }

        public async Task<string> GenerarInforme(int SiniestroId)
        {

            try
            {
                string result = string.Empty;
                var sp = this.Context.LoadStoredProc("dbo.sin_GenerarInforme")
                    .WithSqlParam("SiniestroID", new SqlParameter("SiniestroID", SiniestroId));


                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    result = handler.ReadToList<string>().FirstOrDefault();
                });

                return result;
            }
            catch (Exception ex)
            {
                throw new ValidationException("inf_informesERROR", ex);
            }

        }

        public async Task<List<SinSiniestroAdjuntos>> GetAdjuntosSiniestros(int siniestroId)
        {
            return await Context.SinSiniestroAdjuntos.Where(e => e.SiniestroId == siniestroId).ToListAsync();
        }

        public async Task DeleteFileById(Guid id)
        {
            var adj = await this.Context.SinSiniestroAdjuntos.FirstOrDefaultAsync(e => e.AdjuntoId == id);

            this.Context.SinSiniestroAdjuntos.Remove(adj);

            await this.Context.SaveChangesAsync();
        }

        public async Task<string> GetInforme(string codInforme)
        {
            if (codInforme == null) { return ""; }
            if (codInforme.Trim() == "") { return ""; }
            IList<ItemDto<String>> result = new List<ItemDto<String>>();
            var sp = this.Context.LoadStoredProc("dbo.inf_getNroInformeByCodInforme").WithSqlParam("cod_informe", new SqlParameter("cod_informe", codInforme));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result = handler.ReadToList<ItemDto<String>>();
            });

            return result.ToList().FirstOrDefault()?.Id;

        }
    }
}
