using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinCausas : FullAuditedEntity<int>
    {

        public SinCausas()
        {
            SinSiniestros = new HashSet<SinSiniestros>();
            SinSubCausas = new HashSet<SinSubCausas>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public bool Responsable { get; set; }
        public ICollection<SinSiniestros> SinSiniestros { get; set; }
        public ICollection<SinSubCausas> SinSubCausas { get; set; }
    }
}
