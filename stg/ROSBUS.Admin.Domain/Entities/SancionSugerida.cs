using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSancionSugerida : FullAuditedEntity<int>
    {

        public SinSancionSugerida()
        {
            SinSiniestros = new HashSet<SinSiniestros>();
        }
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinSiniestros> SinSiniestros { get; set; }
    }
}
