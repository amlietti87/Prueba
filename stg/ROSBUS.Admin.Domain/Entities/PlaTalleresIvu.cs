using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTalleresIvu : Entity<int>
    {
        public decimal CodGal { get; set; }
        public int CodGalIvu { get; set; }

        public Galpon CodGalNavigation { get; set; }
    }
}
