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
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using Snickler.EFCore;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdDocumentosProcesadosRepository : RepositoryBase<AdminContext,FdDocumentosProcesados, long>, IFdDocumentosProcesadosRepository
    {

        public FdDocumentosProcesadosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }
        protected override IQueryable<FdDocumentosProcesados> AddIncludeForGet(DbSet<FdDocumentosProcesados> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e=> e.TipoDocumento)
                .Include(e => e.TipoDocumento.FdAcciones)
                .Include("TipoDocumento.FdAcciones.AccionPermitida");
        }
        public override Expression<Func<FdDocumentosProcesados, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        public async Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter)
        {

            var lista = Context.FdDocumentosProcesados.Include(e => e.Estado).Include(e => e.TipoDocumento).ThenInclude(e => e.FdAcciones)
                .Where(e => e.TipoDocumento.FdAcciones.Any(f => (f.MostrarBdempleador == true && f.EstadoActualId == e.EstadoId)
                                                                 || (f.MostrarBdempleador == true && f.EstadoNuevoId == e.EstadoId && f.EsFin == true)));

            if (documentosProcesadosFilter.SucursalId.HasValue)
            {
                lista = lista.Where(e => e.Sucursal.Id == documentosProcesadosFilter.SucursalId);
            }
            if (documentosProcesadosFilter.EmpresaId.HasValue)
            {
                lista = lista.Where(e => e.Empresa.Id == documentosProcesadosFilter.EmpresaId);
            }
            if (documentosProcesadosFilter.FechaDesde.HasValue && documentosProcesadosFilter.FechaHasta == null)
            {
                lista = lista.Where(e => e.Fecha >= documentosProcesadosFilter.FechaDesde);
            }
            if (documentosProcesadosFilter.FechaHasta.HasValue && documentosProcesadosFilter.FechaDesde == null)
            {
                lista = lista.Where(e => e.Fecha <= documentosProcesadosFilter.FechaHasta);
            }
            if (documentosProcesadosFilter.FechaHasta.HasValue && documentosProcesadosFilter.FechaDesde.HasValue)
            {
                lista = lista.Where(e => e.Fecha <= documentosProcesadosFilter.FechaHasta);
            }
            if (documentosProcesadosFilter.TipoDocumentoId.HasValue)
            {
                lista = lista.Where(e => e.TipoDocumentoId == documentosProcesadosFilter.TipoDocumentoId);
            }
            if (documentosProcesadosFilter.EmpleadoId != null)
            {
                lista = lista.Where(e => e.EmpleadoId == documentosProcesadosFilter.EmpleadoId.Id);
            }
            if (documentosProcesadosFilter.EstadoId.HasValue)
            {
                lista = lista.Where(e => e.EstadoId == documentosProcesadosFilter.EstadoId);
            }
            if (documentosProcesadosFilter.Rechazado.HasValue)
            {
                if (documentosProcesadosFilter.Rechazado == 1)
                {
                    lista = lista.Where(e => e.Rechazado == true);
                }
                if (documentosProcesadosFilter.Rechazado == 2)
                {
                    lista = lista.Where(e => e.Rechazado == false);
                }
                
            }
            if (documentosProcesadosFilter.Cerrado.HasValue)
            {
                if (documentosProcesadosFilter.Cerrado == 1)
                {
                    lista = lista.Where(e => e.Cerrado == true);
                }
                if (documentosProcesadosFilter.Cerrado == 2)
                {
                    lista = lista.Where(e => e.Cerrado == false);
                }
                
            }
                //.GroupBy(f => f.Estado.Descripcion).Select(f=> new ArchivosTotalesPorEstado() { Estado = f.Key, Cantidad = f.Count()}).ToListAsync();

           return await lista.GroupBy(f => f.Estado.Descripcion).Select(f => new ArchivosTotalesPorEstado() { Estado = f.Key, Cantidad = f.Count() }).ToListAsync();
        }


        public async Task<List<FdDocumentosProcesados>> ExportarExcel(FdDocumentosProcesadosFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<FdDocumentosProcesados> query = Context.Set<FdDocumentosProcesados>().Where(filter.GetFilterExpression()).AsQueryable();


                query = this.GetIncludesForPageList(query);

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }

                query = query.Include("FdDocumentosProcesadosHistorico.Estado");

                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                //var total = query.Count();


                return new List<FdDocumentosProcesados>(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        public async override Task<PagedResult<FdDocumentosProcesados>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<FdDocumentosProcesados> query = Context.Set<FdDocumentosProcesados>().Where(filter.GetFilterExpression()).AsQueryable();

                var total = await query.CountAsync();


                query = this.GetIncludesForPageList(query);

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }
                query = query.Include(tipodoc => tipodoc.TipoDocumento.FdAcciones)
                             .ThenInclude(navigationPropertyPath: accionperm => accionperm.AccionPermitida)
                             .ThenInclude(token => token.Permiso);

                query = query.Include("FdDocumentosProcesadosHistorico.Estado");

                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                //var total = query.Count();


                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<FdDocumentosProcesados>(total, await list.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        public async Task<List<Destinatario>> GetNotificacionesMail(string token)
        {
            try
            {
                List<Destinatario> result = new List<Destinatario>();
                var sp = this.Context.LoadStoredProc("dbo.GetNotificacionesMail")
                    .WithSqlParam("Token", new SqlParameter("Token", token));


                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    result = handler.ReadToList<Destinatario>().ToList();
                });

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SaveDocumentosImportados(List<FdDocumentosProcesados> procesadosCorrectos, List<FdDocumentosError> procesadosError)
        {
            foreach (var item in procesadosCorrectos)
            {
                this.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }

            foreach (var item in procesadosError)
            {
                this.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }

            this.Context.SaveChanges();
        }

        public void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmador fdFirmador)
        {
            if (fdFirmador != null)
            {
                foreach (var item in fdFirmador.FdFirmadorDetalle)
                {
                    this.Context.Entry(item).State = EntityState.Modified;
                }

                foreach (var item in fdFirmador.FdFirmadorLog.Where(e => e.Id == 0).Reverse())
                {
                    //item.Firmador = fdFirmador;
                    item.FirmadorId = fdFirmador.Id;
                    this.Context.Entry(item).State = EntityState.Added;
                }
            }

            this.Context.Entry(doc).State = EntityState.Modified;

            this.Context.Entry(historico).State = EntityState.Added;

            this.Context.SaveChanges();
        }

        public override async Task<PagedResult<FdDocumentosProcesados>> GetAllAsync(Expression<Func<FdDocumentosProcesados, bool>> predicate, List<Expression<Func<FdDocumentosProcesados, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<FdDocumentosProcesados> query = Context.Set<FdDocumentosProcesados>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include("FdDocumentosProcesadosHistorico.Usuario");
                query = query.Include("FdDocumentosProcesadosHistorico.Estado");

                return new PagedResult<FdDocumentosProcesados>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
