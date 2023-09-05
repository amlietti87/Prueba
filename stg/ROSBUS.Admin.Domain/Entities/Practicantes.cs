using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinPracticantes : FullAuditedEntity<int>
    {

        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public string Celular { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? LocalidadId { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }

        public bool Anulado { get; set; }

        public TipoDni TipoDoc { get; set; }
        public ICollection<SinSiniestros> SinSiniestros { get; set; }
    }
}
