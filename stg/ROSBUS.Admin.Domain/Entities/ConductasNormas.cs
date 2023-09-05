using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinConductasNormas : FullAuditedEntity<int>
    {

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinSiniestros> SinSiniestros { get; set; }
    }
}
