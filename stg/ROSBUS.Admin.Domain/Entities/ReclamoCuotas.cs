using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinReclamoCuotas : FullAuditedEntity<int>
    {
        public int ReclamoId { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public decimal? Monto { get; set; }
        public string Concepto { get; set; }

        public SinReclamos Reclamo { get; set; }
    }
}
