using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSubCausas : FullAuditedEntity<int>
    {

        public int CausaId { get; set; }
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public SinCausas Causa { get; set; }
        public ICollection<SinSiniestros> SinSiniestros { get; set; }
    }
}
