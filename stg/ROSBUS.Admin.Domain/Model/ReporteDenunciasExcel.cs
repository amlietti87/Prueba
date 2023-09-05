using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{
    public class ReporteDenunciasExcel
    {
        public string NroDenuncia { get; set; }
        public string Sucursal { get; set; }
        public string Estado { get; set; }
        public string Empresa { get; set; }
        public string Empleado { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public string EmpleadoEmpresa { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public string EmpleadoArea { get; set; }
        public DateTime? FechaOcurrencia { get; set; }
        public DateTime? FechaRecepcionDenuncia { get; set; }
        public string DenunciaOrigen { get; set; }
        public string EmpleadoDNI { get; set; }
        public string EmpleadoCUIL { get; set; }
        public string EmpleadoFechaEgreso { get; set; }
        public DateTime? FechaProbableAlta { get; set; }
        public string Contingencia { get; set; }
        public string Diagnostico { get; set; }
        public string Patologia { get; set; }
        public string PrestadorMedico { get; set; }
        public string BajaServicio { get; set; }
        public DateTime? FechaBajaServicio { get; set; }
        public string Tratamiento { get; set; }
        public DateTime? FechaUltimoControl { get; set; }
        public DateTime? FechaProximaConsulta { get; set; }
        public DateTime? FechaAudienciaMedica { get; set; }
        public string MotivoAudiencia { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public string AltaMedica { get; set; }
        public DateTime? FechaAltaMedica { get; set; }
        public string AltaLaboral { get; set; }
        public DateTime? FechaAltaLaboral { get; set; }
        public string Reingreso { get; set; }
        public int? NroSiniestro { get; set; }
        public int? CantidadDiasBaja { get; set; }
        public string Juicio { get; set; }
        public string Observaciones { get; set; }
        public string NombreEmpleado { get; set; }
        public string MotivoAnulado { get; set; }
        public string Anulado { get; set; }
        public DateTime? FechaAnulado { get; set; }
        public string ObservacionesNotificacion { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public string MotivoNotificacion { get; set; }
    }
}
