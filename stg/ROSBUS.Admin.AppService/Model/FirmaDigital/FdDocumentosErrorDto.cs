using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdDocumentosErrorDto :EntityDto<long>
    {
        public string NombreArchivo { get; set; }
        public int? TipoDocumentoId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? SucursalEmpleadoId { get; set; }
        public decimal? EmpresaEmpleadoId { get; set; }
        public string LegajoEmpleado { get; set; }
        public string Cuilempleado { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime FechaProcesado { get; set; }
        public string DetalleError { get; set; }
        public bool Revisado { get; set; }
        public string NombreEmpleado { get; set; }
        public override string Description => this.NombreArchivo;

        //Custom props
        public string TipoDocumentoDescripcion { get; set; }
        public string EmpresaDescripcion { get; set; }
        public string SucursalDescripcion { get; set; }
    }
}
