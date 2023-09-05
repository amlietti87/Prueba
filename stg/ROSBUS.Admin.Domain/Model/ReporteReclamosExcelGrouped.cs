using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{
    public class ReporteReclamosExcelGrouped
    {
        public string TipoReclamo { get; set; }
        public DateTime? Fecha { get; set; }
        [Display(Name = "U. de Negocio")]
        public string UnidadNegocio { get; set; }
        public string Estado { get; set; }
        public string SubEstado { get; set; }
        public string Empresa { get; set; }
        [Display(Name = "Siniestro")]
        public string NroSiniestro { get; set; }
        public string Involucrado { get; set; }
        public string NombreEmpleado { get; set; }
        public string EmpleadoDNI { get; set; }
        public string EmpleadoCUIL { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public string EmpleadoEmpresa { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public string EmpleadoArea { get; set; }
        public string EmpleadoFechaEgreso { get; set; }
        public string NroDenuncia { get; set; }
        public decimal? MontoDemandado { get; set; }
        public DateTime? FechaOfrecimiento { get; set; }
        public decimal? MontoOfrecido { get; set; }
        public decimal? MontoReconsideracion { get; set; }
        public string CausaReclamo { get; set; }
        public string Hechos { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal? MontoPagado { get; set; }
        public string PagoEnCuotas { get; set; }
        public decimal? MontoFranquicia { get; set; }
        [Display(Name = "Abogado")]
        public string ApellidoNombre { get; set; }
        public decimal? MontoHonorariosAbogado { get; set; }
        public decimal? MontoHonorariosMediador { get; set; }
        public decimal? MontoHonorariosPerito { get; set; }
        public string JuntaMedica { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public string TipoAcuerdo { get; set; }
        public string RubroSalarial { get; set; }
        public decimal? MontoTasasJudiciales { get; set; }
        public string Autos { get; set; }
        public string NroExpediente { get; set; }
        public string Juzgado { get; set; }
        public string AbogadoEmpresa { get; set; }
        public string Observaciones { get; set; }
        public string ObsConvenioExtrajudicial { get; set; }
        public string MotivoAnulado { get; set; }
        public string Anulado { get; set; }
        public DateTime? FechaCuota { get; set; }
        public decimal? MontoCuota { get; set; }
        public string ConceptoCuota { get; set; }

    }
}
