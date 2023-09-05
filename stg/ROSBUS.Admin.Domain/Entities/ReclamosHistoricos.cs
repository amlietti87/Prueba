using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinReclamosHistoricos : FullAuditedEntity<int, SysUsersAd>
    {

        public int ReclamoId { get; set; }
        public int? InvolucradoId { get; set; }
        public DateTime Fecha { get; set; }
        public int EstadoId { get; set; }
        public int? SiniestroId { get; set; }
        public int SubEstadoId { get; set; }
        public decimal? MontoDemandado { get; set; }
        public DateTime? FechaOfrecimiento { get; set; }
        public decimal? MontoOfrecido { get; set; }
        public decimal? MontoReconsideracion { get; set; }
        public bool? Cuotas { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal? MontoPagado { get; set; }
        public decimal? MontoFranquicia { get; set; }
        public int? AbogadoId { get; set; }
        public int? TipoReclamoId { get; set; }
        public int? DenunciaId { get; set; }
        public decimal? MontoHonorariosAbogado { get; set; }
        public decimal? MontoHonorariosMediador { get; set; }
        public decimal? MontoHonorariosPerito { get; set; }
        public decimal? MontoTasasJudiciales { get; set; }
        public bool? JuntaMedica { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public string Observaciones { get; set; }
        public string ObsConvenioExtrajudicial { get; set; }
        public string Autos { get; set; }
        public string NroExpediente { get; set; }
        public int? JuzgadoId { get; set; }
        public int? AbogadoEmpresaId { get; set; }
        public int? RubroSalarialId { get; set; }
        public int? CausaId { get; set; }
        public int? SucursalId { get; set; }
        public decimal? EmpresaId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? TipoAcuerdoId { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public decimal? EmpleadoEmpresaId { get; set; }
        public string EmpleadoArea { get; set; }
        public string Hechos { get; set; }

        public string NombreEmpleado { get; set; }
        public SinSiniestros Siniestro { get; set; }
        public ArtDenuncias Denuncia { get; set; }
        public Empresa EmpleadoEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public Sucursales Sucursal { get; set; }

        public SinAbogados Abogado { get; set; }
        public SinAbogados AbogadoEmpresa { get; set; }
        public SinEstados Estado { get; set; }
        public SinInvolucrados Involucrado { get; set; }
        public TiposReclamo TipoReclamo { get; set; }
        public SinReclamos Reclamo { get; set; }
        public SinJuzgados Juzgado { get; set; }
        public SinSubEstados SubEstado { get; set; }
        public TiposAcuerdo TipoAcuerdo { get; set; }
        public CausasReclamo Causa { get; set; }
        public RubrosSalariales RubroSalarial { get; set; }
    }
}
