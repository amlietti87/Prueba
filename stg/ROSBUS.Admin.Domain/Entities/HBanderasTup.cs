using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HBanderasTup: Entity<int>
    {
        public HBanderasTup()
        {
            PlaRamalColor = new HashSet<PlaRamalColor>();
        }

        
        public string Descripcion { get; set; }

        public ICollection<PlaRamalColor> PlaRamalColor { get; set; }
    }
}
