using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinDetalleLesion : Entity<int>
    {
        public int InvolucradoId { get; set; }
        public string LugarAtencion { get; set; }
        public DateTime? FechaFactura { get; set; }
        public string Observaciones { get; set; }

        public string NroFactura { get; set; }
        public Decimal? ImporteFactura { get; set; }

        public SinInvolucrados Involucrado { get; set; }
    }
}
