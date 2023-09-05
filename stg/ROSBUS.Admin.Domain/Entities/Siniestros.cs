using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using ROSBUS.Admin.Domain.Entities.ART;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSiniestros : FullAuditedEntity<int,SysUsersAd>
    {

        public SinSiniestros()
        {
            SinInvolucrados = new HashSet<SinInvolucrados>();
            SinSiniestroAdjuntos = new HashSet<SinSiniestroAdjuntos>();
            SinSiniestrosConsecuencias = new HashSet<SinSiniestrosConsecuencias>();
            ArtDenuncias = new HashSet<ArtDenuncias>();
            Reclamos = new HashSet<SinReclamos>();
            ReclamosHistoricos = new HashSet<SinReclamosHistoricos>();
        }

        public int SucursalId { get; set; }

        public decimal EmpresaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Dia { get; set; }
        public string CocheId { get; set; }
        public string CocheInterno { get; set; }
        public string EmpPract { get; set; }
        public int? ConductorId { get; set; }
        public int? PracticanteId { get; set; }
        public string Lugar { get; set; }
        public string Comentario { get; set; }
        public bool? Responsable { get; set; }
        public bool? Descargo { get; set; }
        public DateTime FechaDenuncia { get; set; }
        public decimal? ConductorEmpresaId { get; set; }
        public decimal? CocheLineaId { get; set; }
        public string NroSiniestro { get; set; }
        public string CostoReparacion { get; set; }
        public int? CausaId { get; set; }
        public int? SubCausaId { get; set; }
        public bool? GenerarInforme { get; set; }
        public string CodInforme { get; set; }
        public string ObsDanios { get; set; }
        public string ObsInterna { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string Localidad { get; set; }
        public int? FactoresId { get; set; }
        public int? ConductaId { get; set; }
        public int? SeguroId { get; set; }
        public string NroSiniestroSeguro { get; set; }
        public string CocheDominio { get; set; }
        public int? CocheFicha { get; set; }
        public string ConductorLegajo { get; set; }
       // public int? BandaSiniestralId { get; set; }
        public int? TipoDanioId { get; set; }
        public bool? InformaTaller { get; set; }
        public int? SancionSugeridaId { get; set; }
        public bool Anulado { get; set; }

        public string NombreConductor { get; set; }
        public string DniConductor { get; set; }
       // public SinBandaSiniestral BandaSiniestral { get; set; }
        public SinCausas Causa { get; set; }
        public SinSubCausas SubCausa { get; set; }
        //public CCoches Coche { get; set; }
        public Linea CocheLinea { get; set; }
        public SinConductasNormas Conducta { get; set; }
        public SinSancionSugerida SancionSugerida { get; set; }

        public Empresa Empresa { get; set; }

        public Empresa ConductorEmpresa { get; set; }
        public SinFactoresIntervinientes Factores { get; set; }
        public SinPracticantes Practicante { get; set; }
        public SinCiaSeguros Seguro { get; set; }
        public Sucursales Sucursal { get; set; }
        public SinTipoDanio TipoDanio { get; set; }
        public SinTipoColision TipoColision { get; set; }
        //public Empleados Empleado { get; set; }
        public ICollection<SinReclamos> Reclamos { get; set; }

        public ICollection<SinReclamosHistoricos> ReclamosHistoricos { get; set; }
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }
        public ICollection<SinSiniestroAdjuntos> SinSiniestroAdjuntos { get; set; }
        public ICollection<SinSiniestrosConsecuencias> SinSiniestrosConsecuencias { get; set; }
        public string Direccion { get; set; } 
        public int? CroquiId { get; set; }
        public int? TipoColisionId { get; set; }
        public ICollection<ArtDenuncias> ArtDenuncias { get; set; }

        public string getDescription()
        {
            string result;

            result = string.Format("{0} - {1} - {2} - {3}", this.NroSiniestro.TrimOrNull(), this.Sucursal?.DscSucursal.TrimOrNull(), this.Fecha.ToString("dd/MM/yyyy"), this.Hora.ToString(@"hh\:mm"));
            return result;

        }
    }
}
