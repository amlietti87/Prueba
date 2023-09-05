using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinFactoresIntervinientes : FullAuditedEntity<int>
    {

        public SinFactoresIntervinientes()
        {
            SinSiniestros = new HashSet<SinSiniestros>();
        }
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinSiniestros> SinSiniestros { get; set; }
    }
}
