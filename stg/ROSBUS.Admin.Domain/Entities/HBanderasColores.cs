using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{ 
    public partial class HBanderasColores : Entity<int>
    {
        // public int CodBanderaColor { get; set; }
        public string DscBanderaColor { get; set; }
        public int? CodBanderaColorbsas { get; set; }
    }
}
