using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SubGalpon : Entity<decimal>
    {
        public SubGalpon()
        {
            HHorariosConfi = new HashSet<HHorariosConfi>();
            Configu = new HashSet<Configu>();
        }

        //public decimal CodSubg { get; set; }
        public string DesSubg { get; set; }
        public DateTime? FecBaja { get; set; }
        public string Balanceo { get; set; }
        public string FltComodines { get; set; }

        public ICollection<Configu> Configu { get; set; }
        public ICollection<HHorariosConfi> HHorariosConfi { get; set; }
    }
}
