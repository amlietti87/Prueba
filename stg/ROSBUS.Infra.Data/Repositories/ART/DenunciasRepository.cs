using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Interface;
using TECSO.FWK.Infra.Data.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using Snickler.EFCore;
using TECSO.FWK.Domain.Interfaces.Entities;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Infra.Data.Repositories.ART
{
    public class DenunciasRepository : RepositoryBase<AdminContext, ArtDenuncias, int>, IDenunciasRepository

    {
        private readonly ILogger _logger;
        public DenunciasRepository(IAdminDbContext _context, ILogger logger)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            _logger = logger;
        }

        public override Expression<Func<ArtDenuncias, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId)
        {
            List<HistorialDenuncias> denuncias = new List<HistorialDenuncias>();
            var prestadores = await this.Context.ArtPrestadoresMedicos.ToListAsync();
            var denunciasporempleado = await this.Context.ArtDenuncias.Where(e => e.EmpleadoId == EmpleadoId && e.PrestadorMedicoId.HasValue).Select(e => e.PrestadorMedicoId).Distinct().ToListAsync();
            foreach (var item in denunciasporempleado)
            {
                if (item.HasValue)
                {
                    HistorialDenuncias denuncia = new HistorialDenuncias();
                    var cantidad = await this.Context.ArtDenuncias.Where(e => e.EmpleadoId == EmpleadoId && e.PrestadorMedicoId == item).ToListAsync();
                    denuncia.Prestador = prestadores.Where(e => e.Id == item.Value).FirstOrDefault()?.Descripcion;
                    denuncia.Total = cantidad.Count;
                    denuncia.UltimoAnio = cantidad.Where(e => e.FechaOcurrencia > DateTime.Now.AddYears(-1)).Count();
                    denuncias.Add(denuncia);
                }
            }
            HistorialDenuncias denunciatotal = new HistorialDenuncias();
            denunciatotal.Prestador = "Total";
            denunciatotal.Total = denuncias.Sum(e => e.Total);
            denunciatotal.UltimoAnio = denuncias.Sum(e => e.UltimoAnio);
            denuncias.Add(denunciatotal);
            return denuncias;
        }

        public async Task<List<Destinatario>> GetNotificacionesMail(string Token)
        {

            try
            {
                List<Destinatario> result = new List<Destinatario>();
                var sp = this.Context.LoadStoredProc("dbo.GetNotificacionesMail")
                    .WithSqlParam("Token", new SqlParameter("Token", Token));


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

        public async Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId)
        {
            List<HistorialReclamosEmpleado> reclamos = new List<HistorialReclamosEmpleado>();
            var estados = await this.Context.SinEstados.ToListAsync();
            var reclamosporempleado = await this.Context.SinReclamos.Where(e => e.EmpleadoId == EmpleadoId).Select(e => e.EstadoId).Distinct().ToListAsync();
            foreach (var item in reclamosporempleado)
            {
                HistorialReclamosEmpleado reclamo = new HistorialReclamosEmpleado();
                var cantidad = await this.Context.SinReclamos.Where(e => e.EmpleadoId == EmpleadoId && e.EstadoId == item).ToListAsync();
                reclamo.Estado = estados.Where(e => e.Id == item).FirstOrDefault()?.Descripcion;
                reclamo.Cantidad = cantidad.Count;
                reclamos.Add(reclamo);
            }
            HistorialReclamosEmpleado reclamototal = new HistorialReclamosEmpleado();
            reclamototal.Estado = "Total";
            reclamototal.Cantidad = reclamos.Sum(e => e.Cantidad);
            reclamos.Add(reclamototal);
            return reclamos;
        }

        public async Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId)
        {
            List<HistorialDenunciasPorEstado> denuncias = new List<HistorialDenunciasPorEstado>();
            var estados = await this.Context.ArtEstados.ToListAsync();
            var denunciasporempleado = await this.Context.ArtDenuncias.Where(e => e.EmpleadoId == EmpleadoId).Select(e => e.EstadoId).Distinct().ToListAsync();
            foreach (var item in denunciasporempleado)
            {
                HistorialDenunciasPorEstado denuncia = new HistorialDenunciasPorEstado();
                var cantidad = await this.Context.ArtDenuncias.Where(e => e.EmpleadoId == EmpleadoId && e.EstadoId == item).ToListAsync();
                denuncia.Estado = estados.Where(e => e.Id == item).FirstOrDefault()?.Descripcion;
                denuncia.Cantidad = cantidad.Count;
                denuncias.Add(denuncia);
            }
            HistorialDenunciasPorEstado denunciatotal = new HistorialDenunciasPorEstado();
            denunciatotal.Estado = "Total";
            denunciatotal.Cantidad = denuncias.Sum(e => e.Cantidad);
            denuncias.Add(denunciatotal);
            return denuncias;
        }

        protected override IQueryable<ArtDenuncias> AddIncludeForGet(DbSet<ArtDenuncias> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Sucursal)
                .Include(e => e.Empresa)
                .Include(e => e.Contingencia)
                .Include(e => e.Patologia)
                .Include(e => e.PrestadorMedico)
                .Include(e => e.Tratamiento)
                .Include(e => e.MotivoAudiencia)
                .Include(e => e.CreatedUser)
                .Include(e => e.ArtDenunciasNotificaciones)
                .Include(e => e.Estado)
                .Include(e => e.Siniestro);
        }

        public async Task<List<ArtDenunciaAdjuntos>> GetAdjuntosDenuncias(int DenunciaId)
        {
            return await Context.ArtDenunciaAdjuntos.Where(e => e.DenunciaId == DenunciaId).ToListAsync();
        }

        public async Task DeleteFileById(Guid id)
        {
            var adj = await this.Context.ArtDenunciaAdjuntos.FirstOrDefaultAsync(e => e.AdjuntoId == id);

            this.Context.ArtDenunciaAdjuntos.Remove(adj);

            await this.Context.SaveChangesAsync();
        }

        public async Task<ArtDenuncias> Anular(ArtDenuncias denuncia)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    this.Context.Entry(denuncia).State = EntityState.Modified;

                    await this.Context.SaveChangesAsync();
                    ts.Commit();

                }

                return denuncia;

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }


        }

        public override async Task<TECSO.FWK.Domain.Entities.PagedResult<ArtDenuncias>> GetAllAsync(Expression<Func<ArtDenuncias, bool>> predicate, List<Expression<Func<ArtDenuncias, object>>> includeExpression = null)
        {
            return await base.GetAllAsync(predicate, includeExpression);
        }

        public async Task ImportarDenuncias(List<ImportadorExcelDenuncias> planilla)
        {
            try
            {

                List<ArtDenuncias> allDenuncias = new List<ArtDenuncias>();

                foreach (var item in planilla)
                {
                    allDenuncias.AddRange(this.Context.ArtDenuncias.Where(e => e.NroDenuncia.Trim() == item.NroDenuncia && e.PrestadorMedicoId == item.PrestadorMedicoId && e.Anulado == false).ToList());
                }


                //using (var ts = await this.Context.Database.BeginTransactionAsync(isolationLevel: System.Data.IsolationLevel.Snapshot))
                {
                    foreach (var item in planilla)
                    {
                        var addorupdate = allDenuncias.Where(e => e.NroDenuncia.Trim() == item.NroDenuncia && e.PrestadorMedicoId == item.PrestadorMedicoId && e.Anulado == false).ToList();
                        ArtDenuncias denuncia = null;
                        if (addorupdate.Count == 0)
                        {
                            denuncia = new ArtDenuncias();
                            denuncia.Id = 0;

                            denuncia.EmpleadoId = item.EmpleadoId;
                            denuncia.EmpleadoAntiguedad = item.EmpleadoAntiguedad;
                            denuncia.EmpleadoArea = item.EmpleadoArea;
                            denuncia.EmpleadoEmpresaId = item.EmpleadoEmpresaId;
                            denuncia.EmpleadoFechaIngreso = item.EmpleadoFechaIngreso;
                            denuncia.EmpleadoLegajo = item.EmpleadoLegajo;
                            denuncia.EmpresaId = item.EmpresaId;

                            if (item.MotivoNotificacionId.HasValue || item.FechaNotificacion.HasValue)
                            {
                                denuncia.ArtDenunciasNotificaciones = new List<ArtDenunciasNotificaciones>();
                                ArtDenunciasNotificaciones notificacion = new ArtDenunciasNotificaciones();
                                notificacion.DenunciaId = 0;
                                notificacion.Observaciones = item.ObservacionesNotificacion;
                                notificacion.Id = 0;
                                notificacion.Fecha = item.FechaNotificacion.Value;
                                notificacion.MotivoId = item.MotivoNotificacionId.Value;
                                denuncia.ArtDenunciasNotificaciones.Add(notificacion);
                                //Context.Entry(notificacion).State = EntityState.Added;
                            }
                        }
                        else
                        {
                            denuncia = addorupdate.FirstOrDefault();

                            if (item.MotivoNotificacionId.HasValue || item.FechaNotificacion.HasValue)
                            {
                                
                                ArtDenunciasNotificaciones notificacion = new ArtDenunciasNotificaciones();
                                notificacion.DenunciaId = denuncia.Id;
                                notificacion.Observaciones = item.ObservacionesNotificacion;
                                notificacion.Id = 0;
                                notificacion.Fecha = item.FechaNotificacion.Value;
                                notificacion.MotivoId = item.MotivoNotificacionId.Value;
                                //denuncia.ArtDenunciasNotificaciones.Add(notificacion);
                                Context.ArtDenunciasNotificaciones.Add(notificacion);
                                //Context.Entry(notificacion).State = EntityState.Added;
                            }
                        }


                        denuncia.AltaLaboral = item.FechaAltaLaboral.HasValue;
                        denuncia.AltaMedica = item.FechaAltaMedica.HasValue;
                        denuncia.Anulado = false;
                        denuncia.BajaServicio = item.FechaBajaServicio.HasValue;
                        denuncia.CantidadDiasBaja = item.CantidadDiasBaja;
                        denuncia.ContingenciaId = item.ContingenciaId;
                        denuncia.DenunciaIdOrigen = item.DenunciaOrigenId;
                        denuncia.Diagnostico = item.Diagnostico;

                        denuncia.EstadoId = item.EstadoId;
                        denuncia.FechaAltaLaboral = item.FechaAltaLaboral;
                        denuncia.FechaAltaMedica = item.FechaAltaMedica;
                        denuncia.FechaAudienciaMedica = item.FechaAudienciaMedica;
                        denuncia.FechaBajaServicio = item.FechaBajaServicio;
                        denuncia.FechaOcurrencia = item.FechaOcurrencia.Value;
                        denuncia.FechaProbableAlta = item.FechaProbableAlta;
                        denuncia.FechaProximaConsulta = item.FechaProximaConsulta;
                        denuncia.FechaRecepcionDenuncia = item.FechaRecepcionDenuncia.Value;
                        denuncia.FechaUltimoControl = item.FechaUltimoControl;
                        denuncia.MotivoAnulado = null;
                        denuncia.MotivoAudienciaId = item.MotivoAudienciaId;
                        denuncia.NroDenuncia = item.NroDenuncia;
                        denuncia.Observaciones = item.Observaciones;
                        denuncia.PatologiaId = item.PatologiaId;
                        if (!String.IsNullOrWhiteSpace(item.PorcentajeIncapacidad))
                        {
                            denuncia.PorcentajeIncapacidad = Decimal.Parse(item.PorcentajeIncapacidad);
                        }
                        else
                        {
                            denuncia.PorcentajeIncapacidad = null;
                        }
                        denuncia.PrestadorMedicoId = item.PrestadorMedicoId;
                        denuncia.SiniestroId = item.SiniestroId;
                        denuncia.SucursalId = item.SucursalId;
                        denuncia.TratamientoId = item.TratamientoId;

                        


                        if (denuncia.Id == 0)
                        {
                            //Context.Entry(denuncia).State = EntityState.Added;
                            Context.ArtDenuncias.Add(denuncia);
                            await this._logger.LogInformation(String.Format("Se procede a realizar un insert, NroDenuncia: {0} - Prestador Médico ID: {1}", denuncia.NroDenuncia, denuncia.PrestadorMedicoId));
                        }
                        else
                        {
                            //Context.Entry(denuncia).State = EntityState.Modified;
                            await this._logger.LogInformation(String.Format("Se procede a realizar un update en la denuncia con Id: {0}", denuncia.Id));
                        }
                        
                    }


                    await this.Context.SaveChangesAsync();
                    //ts.Commit();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        public async Task<List<ReporteDenunciasExcel>> GetReporteExcel(ExcelDenunciasFilter filter)
        {
            List<ReporteDenunciasExcel> items = new List<ReporteDenunciasExcel>();

            var sp = this.Context.LoadStoredProc("dbo.sp_ARTDenuncias_ReporteDenunciasExcel")
                .WithSqlParam("ids", new SqlParameter("ids", filter.Ids));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                items = handler.ReadToList<ReporteDenunciasExcel>().ToList();
            });

            return items;
        }
    }
}
