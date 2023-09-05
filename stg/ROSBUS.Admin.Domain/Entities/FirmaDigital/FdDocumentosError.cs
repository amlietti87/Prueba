using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public class FdDocumentosError : AuditedEntity<long>
    {

        public FdDocumentosError()
        {
            Errors = new List<string>();
        }
        public string NombreArchivo { get; set; }
        public int? TipoDocumentoId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? SucursalEmpleadoId { get; set; }
        public decimal? EmpresaEmpleadoId { get; set; }
        public string LegajoEmpleado { get; set; }
        public string Cuilempleado { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaProcesado { get; set; }
        public string DetalleError { get; set; }
        public bool Revisado { get; set; }
        public string NombreEmpleado { get; set; }
        public Sucursales Sucursal { get; set; }

        public Empresa Empresa { get; set; }
        public FdTiposDocumentos TipoDocumento { get; set; }


        [NotMapped]
        public string File { get; set; }

        
        private List<string> Errors { get; set; }

        public List<string> GetErrors()
        {
            return this.Errors;
        }

        public void AddErrors(List<string> errors)
        {

            this.DetalleError = string.Join(char.ConvertFromUtf32(13), errors);
            this.Errors = errors;
        }
    }
}
