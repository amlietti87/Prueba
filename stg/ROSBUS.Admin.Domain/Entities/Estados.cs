using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinEstados : FullAuditedEntity<int>
    {
        public SinEstados()
        {
            SinReclamos = new HashSet<SinReclamos>();
            SinSubEstados = new HashSet<SinSubEstados>();
        }

        public string Descripcion { get; set; }
        public int? OrdenCambioEstado { get; set; }
        public bool Judicial { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinReclamos> SinReclamos { get; set; }
        public ICollection<SinReclamosHistoricos> SinReclamosHistoricos { get; set; }
        public ICollection<SinSubEstados> SinSubEstados { get; set; }
    }
}
