using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{
    public class ExportarExcelFirmaDigital
    {
        public string TipoDocumento { get; set; }
        public string Fecha { get; set; }

        public string FechaProcesado { get; set; }

        public string Empleado { get; set; }

        public string Legajo { get; set; }

        public string Cuil { get; set; }

        public string UnidadNegocio { get; set; }

        public string Empresa { get; set; }

        public string Estado { get; set; }

        public string EstadoConformidad { get; set; }

        public string Rechazado { get; set; }

        public string MotivoRechazo { get; set; }

        public string Cerrado { get; set; }

        public string NombreArchivo { get; set; }
    }
}
