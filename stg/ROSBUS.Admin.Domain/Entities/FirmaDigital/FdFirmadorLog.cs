using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class FdFirmadorLog : Entity<long>
    {
        public long FirmadorId { get; set; }
        public string DetalleLog { get; set; }
        public DateTime FechaHora { get; set; }

        public virtual FdFirmador Firmador { get; set; }
    }
}
