using ROSBUS.Admin.AppService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenunciasDto : EntityDto<int>
    {
        public ArtDenunciasDto()
        {
            //this.ArtDenunciaAdjuntosDto = new List<ArtDenunciaAdjuntosDto>();
            //this.ArtDenunciasEstadosDto = new List<ArtDenunciasEstadosDto>();
            this.DenunciaNotificaciones = new List<ArtDenunciasNotificacionesDto>();
        }
        [Required]
        public string NroDenuncia { get; set; }
        [Required]
        public int EmpresaId { get; set; }
        [Required]
        public int EstadoId { get; set; }
        [Required]
        public int EmpleadoId { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public int? EmpleadoEmpresaId { get; set; }

        public int? DenunciaIdOrigen { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public string EmpleadoArea { get; set; }
        [Required]
        public DateTime FechaOcurrencia { get; set; }
        [Required]
        public DateTime FechaRecepcionDenuncia { get; set; }
        public int? ContingenciaId { get; set; }
        public string Diagnostico { get; set; }
        public int? PatologiaId { get; set; }
        [Required]
        public int? PrestadorMedicoId { get; set; }
        public bool? BajaServicio { get; set; }
        public DateTime? FechaBajaServicio { get; set; }
        public int? TratamientoId { get; set; }
        public DateTime? FechaUltimoControl { get; set; }
        public DateTime? FechaProximaConsulta { get; set; }
        public DateTime? FechaAudienciaMedica { get; set; }
        public int? MotivoAudienciaId { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public bool? AltaMedica { get; set; }
        public DateTime? FechaAltaMedica { get; set; }
        public bool? AltaLaboral { get; set; }
        public DateTime? FechaAltaLaboral { get; set; }
        public bool? TieneReingresos { get; set; }
        public int? DenunciaId { get; set; }
        public int? SiniestroId { get; set; }
        public bool? Juicio { get; set; }
        public string Observaciones { get; set; }
        [Required]
        public bool Anulado { get; set; }

        public string NombreEmpleado { get; set; }
        public string SucursalGrilla { get; set; }
        public string EmpresaGrilla { get; set; }
        public int CantidadDiasBaja { get; set; }
        public string MotivoAnulado { get; set; }

        public string CreatedUserName { get; set; }

        public string DniEmpleado { get; set; }

        public string Color { get; set; }
        public DateTime? FechaProbableAlta { get; set; }
        public ArtContingenciasDto Contingencia { get; set; }
        public ArtPatologiasDto Patologia { get; set; }
        public ArtPrestadoresMedicosDto PrestadorMedico { get; set; }
        public ArtTratamientosDto Tratamiento { get; set; }
        public ArtMotivosAudienciasDto MotivoAudiencia { get; set; }
       // public ArtDenunciasDto Denuncia { get; set; }
        public ItemDto<int> selectedEmpleado => getItemEmpleado();
        public string EmpleadoGrilla => GetGrillaEmpleado();
        //public ArtEstadosDto EstadoDto { get; set; }
        //[NotMapped]
        //public EmpresaDto EmpleadoEmpresaDto { get; set; }
        //[NotMapped]
        //public EmpresaDto EmpresaDto { get; set; }
        //public ArtMotivosAudienciasDto MotivoAudienciaDto { get; set; }

        //public ArtPrestadoresMedicosDto PrestadorMedicoDto { get; set; }
        public SiniestrosDto Siniestro { get; set; }
        //public ArtTratamientosDto TratamientoDto { get; set; }
        //public List<ArtDenunciaAdjuntosDto> ArtDenunciaAdjuntosDto { get; set; }
        //public List<ArtDenunciasEstadosDto> ArtDenunciasEstadosDto { get; set; }
        public List<ArtDenunciasNotificacionesDto> DenunciaNotificaciones { get; set; }

        public ItemDto<int> selectedSiniestro { get; set; }

        public override string Description => NroDenuncia;
        public int SucursalId { get; set; }
        public string GetGrillaEmpleado()
        {
            return string.Format("{0} - Legajo: {1} - DNI: {2}", NombreEmpleado, EmpleadoLegajo, DniEmpleado);
        }
        public ItemDto<int> getItemEmpleado()
        {

            return new ItemDto<int>(EmpleadoId, string.Format("{0} - {1}", NombreEmpleado, EmpleadoLegajo));
        }
    }
}
