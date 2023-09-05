using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaLineasUsuarios : CreationAuditedEntity<int>
    {
       
        public PlaLineasUsuarios()
        {

        }

        public int UserId { get; set; }
        public decimal CodLin { get; set; }
        public Linea CodLinNavigation { get; set; }
        public SysUsersAd User { get; set; }
    }
}

