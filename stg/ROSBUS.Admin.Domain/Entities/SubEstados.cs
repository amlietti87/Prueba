using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSubEstados : FullAuditedEntity<int>
    {
        public int EstadoId { get; set; }
        public string Descripcion { get; set; }
        public bool Cierre { get; set; }
        public bool Anulado { get; set; }


        public SinEstados Estado { get; set; }
        public ICollection<SinReclamos> SinReclamos { get; set; }

        public ICollection<SinReclamosHistoricos> SinReclamosHistoricos { get; set; }
    }
}
