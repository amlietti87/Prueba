using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Sucursales : Entity<int>
    {

        public Sucursales()
        {
            PlaGrupoLineas = new HashSet<PlaGrupoLineas>();
            PlaLineas = new HashSet<PlaLinea>();
            Configu = new HashSet<Configu>();
        ArtDenuncias = new HashSet<ArtDenuncias>();
            Reclamos = new HashSet<SinReclamos>();
            ReclamosHistoricos = new HashSet<SinReclamosHistoricos>();
            FdDocumentosProcesados = new HashSet<FdDocumentosProcesados>();
            FdDocumentosError = new HashSet<FdDocumentosError>();
        }

        public string DscSucursal { get; set; }
        public string NomServidor { get; set; }
        public string EntornoActivo { get; set; }


        public ICollection<PlaGrupoLineas> PlaGrupoLineas { get; set; }
        public ICollection<SinSiniestros> SinSiniestros { get; set; }
        public ICollection<Configu> Configu { get; set; }
        public ICollection<PlaLinea> PlaLineas { get; set; }
        public ICollection<SinReclamos> Reclamos { get; set; }
        public ICollection<ArtDenuncias> ArtDenuncias { get; set; }
        public ICollection<SinReclamosHistoricos> ReclamosHistoricos { get; set; }
        public ICollection<FdDocumentosProcesados> FdDocumentosProcesados { get; set; }
        public ICollection<FdDocumentosError> FdDocumentosError { get; set; }
    }
}
