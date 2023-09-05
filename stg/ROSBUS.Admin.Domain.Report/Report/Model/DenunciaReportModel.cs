using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Report.Report.Model
{
    public class DenunciaReportModel
    {
        public string NroDenuncia { get; set; }
        public string Estado { get; set; }
        public string Empresa { get; set; }
        public string Empleado { get; set; }
        public string Domicilio { get; set; }
        public string Contingencia { get; set; }
        public string Prestador { get; set; }
        public string Tratamiento { get; set; }
        public string AltaMedica { get; set; }
        public string Diagnostico { get; set; }
        public string Patologia { get; set; }
        public string FechaOcurrencia { get; set; }
        public string FechaRecepcionDenuncia { get; set; }
        public string FechaBajaServicio { get; set; }
        public string FechaUltimoControl { get; set; }
        public string FechaProximaConsultaTurno { get; set; }
        public string FechaAudienciaMedica { get; set; }
        public string FechaProbableAltaMedica { get; set; }
    }
}
