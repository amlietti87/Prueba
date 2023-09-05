using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlanCam: Entity<decimal>
    {
        public PlanCam()
        {
            Configu = new HashSet<Configu>();
        }
        //public decimal PlanCam1 { get; set; }
        public string DesPlan { get; set; }
        public string Depot { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Fecha { get; set; }

        public ICollection<Configu> Configu { get; set; }
    }
}
