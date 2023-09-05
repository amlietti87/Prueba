using System;
using System.Collections.Generic;
using System.Text;
using ROSBUS.Operaciones.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public class ImportadorDocumentosModel
    {

        public ImportadorDocumentosModel()
        {
            Errors = new List<string>();
        }

        public bool IsValid { get; set; }
        public FdTiposDocumentos TipoDocumento { get; set; }
        public string File { get; set; }
        public Empleados Empleado { get; set; }
        public DateTime? Fecha { get; set; }
        public LegajosEmpleado LegajoEmpleado { get; set; }
        private List<string> Errors { get; set; }

        public void AddError(string message)
        {
            Errors.Add(message);
            this.IsValid = false;
        }

        public List<string> GetError()
        {
            return this.Errors;
        }
    }
}
