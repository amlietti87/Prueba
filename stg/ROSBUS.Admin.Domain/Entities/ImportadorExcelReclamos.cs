using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities
{
    public class ImportadorExcelReclamos
    {
        public ImportadorExcelReclamos()
        {
            Errors = new List<string>();
            IsValid = true;
        }
        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }

        //Data from excel
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string TipoReclamo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string SubEstado { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UnidadNegocio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Empresa { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string FechaReclamoExcel { get; set; }
        public string NroDenuncia { get; set; }
        public string EmpleadoDNI { get; set; }
        public string EmpleadoCUIL { get; set; } 
        public string MontoDemandadoExcel { get; set; }
        public string FechaOfrecimientoExcel { get; set; }
        public string MontoOfrecidoExcel { get; set; }
        public string MontoReconsideracionExcel { get; set; }
        public string CausaReclamo { get; set; }
        public string Hechos { get; set; }
        public string FechaPagoExcel { get; set; }
        public string MontoPagadoExcel { get; set; }
        public string MontoFranquiciaExcel { get; set; }
        public string Abogado { get; set; }
        public string HonorariosAbogadoActorExcel { get; set; }
        public string HonorariosMediadorExcel { get; set; }
        public string HonorariosPeritoExcel { get; set; }
        public string JuntaMedicaExcel { get; set; }
        public string PorcIncapacidad { get; set; }
        public string TipoAcuerdo { get; set; }
        public string RubroSalarial { get; set; }
        public string MontoTasasJudicialesExcel { get; set; }
        public string Observaciones { get; set; }
        public string ObsConvExtrajudicial { get; set; }
        public string Autos { get; set; }
        public string NroExpediente { get; set; }
        public string Juzgado { get; set; }
        public string AbogadoEmpresa { get; set; }

        //Properties for mapping
        public int TiposReclamoId { get; set; }
        public int EstadoId { get; set; }
        public int SubEstadoId { get; set; }
        public int SucursalId { get; set; }
        public int? CausaId { get; set; }
        public int? AbogadoId { get; set; }
        public int? TipoAcuerdoId { get; set; }
        public int? RubroSalarialId { get; set; }
        public int? JuzgadoId { get; set; }
        public int? AbogadoEmpresaId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? DenunciaId { get; set; }
        public bool? JuntaMedica { get; set; }
        public decimal EmpresaId { get; set; }
        public decimal? MontoPagado { get; set; }
        public decimal? MontoFranquicia { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public decimal? HonorariosAbogadoActor { get; set; }
        public decimal? HonorariosMediador { get; set; }
        public decimal? HonorariosPerito { get; set; }
        public decimal? MontoDemandado { get; set; }
        public decimal? MontoOfrecido { get; set; }
        public decimal? MontoReconsideracion { get; set; }
        public decimal? MontoTasasJudiciales { get; set; }
        public decimal? EmpleadoEmpresaId { get; set; }
        public DateTime? FechaReclamo { get; set; }
        public DateTime? FechaOfrecimiento { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public string EmpleadoLegajo { get; set; }
        public string EmpleadoArea { get; set; }

    }

    public class ReclamoImportadorFileFilter
    {
        public string PlanillaId { get; set; }

    }
}
