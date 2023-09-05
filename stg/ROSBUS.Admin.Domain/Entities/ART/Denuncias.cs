
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenuncias : FullAuditedEntity<int, SysUsersAd>
    {
        public ArtDenuncias()
        {
            ArtDenunciaAdjuntos = new HashSet<ArtDenunciaAdjuntos>();
            ArtDenunciasNotificaciones = new HashSet<ArtDenunciasNotificaciones>();
            Reclamos = new HashSet<SinReclamos>();
            ReclamosHistoricos = new HashSet<SinReclamosHistoricos>();
            ArtDenunciaReingresos = new HashSet<ArtDenuncias>();
        }

        public string NroDenuncia { get; set; }
        public decimal EmpresaId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public int SucursalId { get; set; }
        public decimal? EmpleadoEmpresaId { get; set; }
        public string EmpleadoArea { get; set; }
        public DateTime FechaOcurrencia { get; set; }
        public DateTime FechaRecepcionDenuncia { get; set; }
        public int? ContingenciaId { get; set; }
        public string Diagnostico { get; set; }
        public int? PatologiaId { get; set; }
        public int? PrestadorMedicoId { get; set; }
        public bool? BajaServicio { get; set; }
        public DateTime? FechaBajaServicio { get; set; }
        public int? TratamientoId { get; set; }
        public int EstadoId { get; set; }
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
        public int? DenunciaIdOrigen { get; set; }
        public int? SiniestroId { get; set; }
        public bool? Juicio { get; set; }
        public string Observaciones { get; set; }
        public bool Anulado { get; set; }
        public int CantidadDiasBaja { get; set; }
        public string NombreEmpleado { get; set; }

        public string MotivoAnulado { get; set; }
        public string DniEmpleado { get; set; }
        public ArtContingencias Contingencia { get; set; }
        public Empresa EmpleadoEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public ArtMotivosAudiencias MotivoAudiencia { get; set; }
        public ArtPatologias Patologia { get; set; }
        public ArtPrestadoresMedicos PrestadorMedico { get; set; }
        public SinSiniestros Siniestro { get; set; }
        public ArtTratamientos Tratamiento { get; set; }
        public ArtEstados Estado { get; set; }
        public Sucursales Sucursal { get; set; }
        public ArtDenuncias DenunciaOrigen { get; set; }

        public DateTime? FechaProbableAlta { get; set; }
        public ICollection<ArtDenuncias> ArtDenunciaReingresos { get; set; }
        public ICollection<ArtDenunciaAdjuntos> ArtDenunciaAdjuntos { get; set; }
        public ICollection<ArtDenunciasNotificaciones> ArtDenunciasNotificaciones { get; set; }
        public ICollection<SinReclamos> Reclamos { get; set; }
        public ICollection<SinReclamosHistoricos> ReclamosHistoricos { get; set; }

        public string getDescription()
        {
            string result;

            result = string.Format("{0} - {1} - {2} - {3}", this.NroDenuncia.TrimOrNull(), this.PrestadorMedico?.Descripcion.TrimOrNull(), this.FechaOcurrencia.ToString("dd/MM/yyyy"), this.FechaRecepcionDenuncia.ToString("dd/MM/yyyy"));
            return result;

        }
    }
}
