using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class DshTipoDasboard : Entity<int>
    {
        public DshTipoDasboard()
        {
            DshDashboard = new HashSet<DshDashboard>();
        }

        public string Descripcion { get; set; }

        public ICollection<DshDashboard> DshDashboard { get; set; }
    }
}
