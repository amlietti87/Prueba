using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class DshDashboard : Entity<int>
    {
        public DshDashboard()
        {
            DshUsuarioDashboardItem = new HashSet<DshUsuarioDashboardItem>();
        }

        public string Descripcion { get; set; }
        public int TipoDashboardId { get; set; }

        public DshTipoDasboard TipoDashboard { get; set; }
        public ICollection<DshUsuarioDashboardItem> DshUsuarioDashboardItem { get; set; }
    }
}
