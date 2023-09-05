using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Grupos : Entity<decimal>
    {

        public Grupos()
        {
            Configu = new HashSet<Configu>();
        }
        //public decimal CodGru { get; set; }
        public string DesGru { get; set; }
        public DateTime? FecBaja { get; set; }

        public ICollection<Configu> Configu { get; set; }
    }
}
