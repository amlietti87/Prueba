using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Empresa : TECSO.FWK.Domain.Entities.Entity<Decimal>
    {
        public Empresa()
        {
            SinSiniestrosEmpresaEmpresa = new HashSet<SinSiniestros>();
            SinSiniestrosConductorEmpresa = new HashSet<SinSiniestros>();
            Coches = new HashSet<CCoches>();
            PlaCodigoSubeBandera = new HashSet<PlaCodigoSubeBandera>();
            Configu = new HashSet<Configu>();

            ArtDenunciasEmpresa = new HashSet<ArtDenuncias>();
            ArtDenunciasEmpleadoEmpresa = new HashSet<ArtDenuncias>();
            ArtReclamosEmpleadoEmpresa = new HashSet<SinReclamos>();
            Reclamos = new HashSet<SinReclamos>();
            ReclamosHistoricos = new HashSet<SinReclamosHistoricos>();
            ReclamosHistoricosEmpleadoEmpresa = new HashSet<SinReclamosHistoricos>();
            DocumentosProcesados = new HashSet<FdDocumentosProcesados>();
            DocumentosError = new HashSet<FdDocumentosError>();
        }

    


        //public decimal CodEmpr { get; set; }
        public string DesEmpr { get; set; }
        public string DomEmpr { get; set; }
        public string PosEmpr { get; set; }
        public string TelEmpr { get; set; }
        public string ActEmpr { get; set; }
        public string DepEmpr { get; set; }
        public string Cuit { get; set; }
        public string Anses { get; set; }
        public decimal? Reducc { get; set; }
        public int? Art { get; set; }
        public int? Zona { get; set; }
        public int? CodActi { get; set; }
        public DateTime? FecBaja { get; set; }
        public string Cuenta { get; set; }
        public string CodIva { get; set; }
        public string Siglas { get; set; }
        public decimal? AlicuotaLRT { get; set; }
        public decimal? CuotaLRT { get; set; }
        public string NroIsib { get; set; }
        public string NroAgenteIb { get; set; }
        public string CtaDeposito { get; set; }
        public string IdSap { get; set; }
        public string CuentaSap { get; set; }
        public string NroSap { get; set; }

        public ICollection<SinReclamos> Reclamos { get; set; }
        public ICollection<PlaCodigoSubeBandera> PlaCodigoSubeBandera { get; set; }
        public ICollection<SinSiniestros> SinSiniestrosEmpresaEmpresa { get; set; }
        public ICollection<SinSiniestros> SinSiniestrosConductorEmpresa { get; set; }

        public ICollection<Configu> Configu { get; set; }
        public ICollection<CCoches> Coches { get; set; }
        // public ICollection<RamalSube> RamalSube { get; set; }
        public ICollection<SinReclamosHistoricos> ReclamosHistoricos { get; set; }
        public ICollection<ArtDenuncias> ArtDenunciasEmpresa { get; set; }
        public ICollection<ArtDenuncias> ArtDenunciasEmpleadoEmpresa { get; set; }
        public ICollection<SinReclamos> ArtReclamosEmpleadoEmpresa { get; set; }
        public ICollection<SinReclamosHistoricos> ReclamosHistoricosEmpleadoEmpresa { get; set; }

        public ICollection<FdDocumentosProcesados> DocumentosProcesados { get; set; }
        public ICollection<FdDocumentosError> DocumentosError { get; set; }
    }
}
