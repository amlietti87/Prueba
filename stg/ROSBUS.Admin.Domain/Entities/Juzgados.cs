using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinJuzgados : FullAuditedEntity<int>
    {

        public string Descripcion { get; set; }
        public int? LocalidadId { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinReclamos> SinReclamos { get; set; }
        public ICollection<SinReclamosHistoricos> SinReclamosHistoricos { get; set; }
    }
}
