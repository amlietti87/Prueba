using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdDocumentosProcesados : AuditedEntity<long>
    {
        public FdDocumentosProcesados()
        {
            FdDocumentosProcesadosHistorico = new HashSet<FdDocumentosProcesadosHistorico>();
        }

        public int TipoDocumentoId { get; set; }
        public int EmpleadoId { get; set; }
        public int SucursalEmpleadoId { get; set; }
        public decimal EmpresaEmpleadoId { get; set; }
        public string LegajoEmpleado { get; set; }
        public string Cuilempleado { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaProcesado { get; set; }
        public Guid ArchivoId { get; set; }
        public int EstadoId { get; set; }
        //public string CodMinisterio { get; set; }
        public bool Rechazado { get; set; }
        public string MotivoRechazo { get; set; }

        public string NombreEmpleado { get; set; }
        public bool Cerrado { get; set; }
        public bool? EmpleadoConforme { get; set; }

        public FdEstados Estado { get; set; }
        public Sucursales Sucursal { get; set; }

        public Empresa Empresa { get; set; }
        public FdTiposDocumentos TipoDocumento { get; set; }
        public ICollection<FdDocumentosProcesadosHistorico> FdDocumentosProcesadosHistorico { get; set; }

        [NotMapped]
        public string File { get; set; }
    }
}
