using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_VerDetalleDocumento : AccionBuilder<dynamic>, IAccion_VerDetalleDocumento
    {
        private readonly IAdjuntosAppService adjuntosAppService;
        public Accion_VerDetalleDocumento(IServiceProvider _serviceProvider, IAdjuntosAppService _adjuntosAppService) : base(_serviceProvider)
        {
            adjuntosAppService = _adjuntosAppService;
        }

        protected override async Task<dynamic> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            var documentosProcesados = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento,
                e => e.Empresa,
                e => e.Estado,
                e => e.Sucursal,
                e => e.FdDocumentosProcesadosHistorico
            })).Items.Select(e => new VerDetalles() {  Fecha = e.Fecha.ToString("dd/MM/yyyy"), NombreEmpleado = e.NombreEmpleado, TipoDocumento = e.TipoDocumento.Descripcion, UNegocio = e.Sucursal.DscSucursal,
                                                      CuilEmpleado = e.Cuilempleado, Empresa = e.Empresa.DesEmpr, Estado = e.Estado.Descripcion, Legajo = e.LegajoEmpleado, FechaProcesado = e.FechaProcesado.ToString("dd/MM/yyyy H:mm"),
                                                      Archivo = e.ArchivoId, Cerrado = e.Cerrado, EmpleadoConforme = e.EmpleadoConforme, Rechazado = e.Rechazado, MotivoRechazo = e.MotivoRechazo,
                                                      Historicos = e.FdDocumentosProcesadosHistorico.Select(d => new HistoricosDocumentos() { EstadoH = d.Estado.Descripcion, FechaHora = d.CreatedDate, RechazadoBoolean = d.Rechazado, Usuario = d.Usuario.NomUsuario }).ToList()}).ToList();
            foreach(var ar in documentosProcesados)
            {
                var arch = await adjuntosAppService.GetByIdAsync(ar.Archivo);
                ar.ArchivoNombre = arch.Nombre;

                ar.Historicos = ar.Historicos.OrderByDescending(e => e.FechaHora).ToList();
            }
            return documentosProcesados;            
        }
    }
    public interface IAccion_VerDetalleDocumento : IAccionBuilder
    {
    }

    public class VerDetalles
    {
        public string Fecha { get; set; }
        public string NombreEmpleado { get; set; }
        public string TipoDocumento { get; set; }
        public string CuilEmpleado { get; set; }
        public string Empresa { get; set; }
        public string Legajo { get; set; }
        public string FechaProcesado { get; set; }
        public string Estado { get; set; }
        public Guid Archivo { get; set; }
        public string ArchivoNombre { get; set;}
        public bool Cerrado { get; set; }
        public bool? EmpleadoConforme { get; set; }
        public bool Rechazado { get; set; }
        public string MotivoRechazo { get; set; }
        public string UNegocio { get; set; }
        public List<HistoricosDocumentos> Historicos { get; set; }
    }

    public class HistoricosDocumentos
    {
        public string EstadoH { get; set; }
        public DateTime? FechaHora { get; set; }
        public string FechaHoraString
        {
            get
            {
                return this.FechaHora?.ToString("dd/MM/yyyy H:mm");
            }
            set
            {
                DateTime fec;
                if (DateTime.TryParse(value, out fec))
                {
                    this.FechaHora = fec;
                }
                else
                {
                    this.FechaHora = null;
                }

            }
        }

        public Boolean? RechazadoBoolean { get; set; }
        public string RechazadoH
        {
            get
            {
                if (this.RechazadoBoolean.HasValue)
                {
                    return this.RechazadoBoolean.Value? "Si" : "No";
                }
                else
                {
                    return "No";
                }
            }
            set { }
        }


        public string Usuario { get; set; }
    }
}