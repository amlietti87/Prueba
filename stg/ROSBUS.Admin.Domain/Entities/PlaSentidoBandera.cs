using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaSentidoBandera : Entity<int>
    {

        public PlaSentidoBandera()
        {
            this.HBanderas= new HashSet<HBanderas>();
        }

        public string Descripcion { get; set; }

        public string Color { get; set; }
        public ICollection<HBanderas> HBanderas { get; set; }

    }
}
