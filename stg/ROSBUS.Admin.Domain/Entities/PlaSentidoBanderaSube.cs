using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaSentidoBanderaSube : Entity<int>
    {
        public string Descripcion { get; set; }

        public PlaSentidoBanderaSube()
        {
            PlaCodigoSubeBandera = new HashSet<PlaCodigoSubeBandera>();
        }

        public ICollection<PlaCodigoSubeBandera> PlaCodigoSubeBandera { get; set; }
    }
}
