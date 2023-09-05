using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities.ART
{


    public class ImportadorExcelDenuncias
    {
        public ImportadorExcelDenuncias()
        {
            Errors = new List<string>();
        }
        public string PrestadorMedico { get; set; }
        public string NroDenuncia { get; set; }
        public string Estado { get; set; }
        public string Empresa { get; set; }
        public string EmpleadoDNI { get; set; }
        public string EmpleadoCUIL { get; set; }
        public DateTime? FechaOcurrencia { get; set; }
        public DateTime? FechaRecepcionDenuncia { get; set; }
        public string Contingencia { get; set; }
        public string Diagnostico { get; set; }
        public string Patologia { get; set; }
        public DateTime? FechaBajaServicio { get; set; }
        public string Tratamiento { get; set; }
        public DateTime? FechaUltimoControl { get; set; }
        public DateTime? FechaProximaConsulta { get; set; }
        public DateTime? FechaAudienciaMedica { get; set; }
        public string MotivoAudiencia { get; set; }
        public string PorcentajeIncapacidad { get; set; }
        public DateTime? FechaAltaMedica { get; set; }
        public DateTime? FechaAltaLaboral { get; set; }
        public DateTime? FechaProbableAlta { get; set; }
        public string NroDenunciaOrigen { get; set; }
        public string NroSiniestro { get; set; }
        public string Observaciones { get; set; }
        public string MotivoNotificacion { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public string ObservacionesNotificacion { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }


        // properties for mapping
        public int EstadoId { get; set; }
        public decimal EmpresaId { get; set; }
        public int EmpleadoId { get; set; }
        public int PrestadorMedicoId { get; set; }
        public int SucursalId { get; set; }
        
        public int? ContingenciaId { get; set; }
        public int? PatologiaId { get; set; }
        public int? TratamientoId { get; set; }
        public int? MotivoAudienciaId { get; set; }

        public int? MotivoNotificacionId { get; set; }
        public int? DenunciaOrigenId { get; set; }
        public int? SiniestroId { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public string EmpleadoArea { get; set; }
        public int? EmpleadoEmpresaId { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public int CantidadDiasBaja { get; set; }
    }

    public class DenunciaImportadorFileFilter
    {
        public string PlanillaId {get;set;}

    }
}
