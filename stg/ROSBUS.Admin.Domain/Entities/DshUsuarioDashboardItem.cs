using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class DshUsuarioDashboardItem : Entity<int>
    {
        
        public int DashboardId { get; set; }
        public int CodUsuario { get; set; }
        public int Columna { get; set; }
        public int Orden { get; set; }

        public SysUsersAd Usuario { get; set; }
        public DshDashboard Dashboard { get; set; }
    }
}
