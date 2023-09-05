using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Linea : FullAuditedEntity<decimal>
    {
        public Linea()
        {
            SucursalesxLineas = new HashSet<SucursalesxLineas>();
            PlaLineasUsuarios = new HashSet<PlaLineasUsuarios>();
            PlaLineaLineaHoraria = new HashSet<PlaLineaLineaHoraria>();
            HFechasConfi = new HashSet<HFechasConfi>();
            Configu = new HashSet<Configu>();
        }

        //public decimal CodLin { get; set; }
        public string DesLin { get; set; }
        public string AsocBan { get; set; }
        public decimal? DiasBaj { get; set; }
        public string UrbInter { get; set; }
        public string ClaEmpr1 { get; set; }
        public string ClaEmpr2 { get; set; }
        public string ClaEmpr3 { get; set; }
        public string ClaEmpr4 { get; set; }
        public DateTime? FecBaja { get; set; }
        public int? CodLinCaudalimetro { get; set; }
        public string IdSap { get; set; }
        public string SociedadFi { get; set; }
        public string GrupoSap { get; set; }
        public string GrupoQv { get; set; }
        public string HoraCambioTurno { get; set; }
        public string BandaHoraria { get; set; }

        // public ICollection<BolBanderasRamalesRel> BolBanderasRamalesRel { get; set; }

        public ICollection<SucursalesxLineas> SucursalesxLineas { get; set; }

        public PlaGrupoLineas PlaGrupoLinea { get; set; }
        public int? PlaGrupoLineaId { get; set; }
        
        public bool? Activo { get; set; }
        public ICollection<Configu> Configu { get; set; }
        public ICollection<HFechasConfi> HFechasConfi { get; set; }

        public ICollection<SinSiniestros> SinSiniestros { get; set; }
        public ICollection<PlaLineasUsuarios> PlaLineasUsuarios { get; set; }
        public ICollection<PlaLineaLineaHoraria> PlaLineaLineaHoraria { get; set; }

        public string CodRespInformes { get; set; }

        public virtual InfResponsables RespInformes { get; set; } // FK_pla_Linea_inf_responsables
    }
}
