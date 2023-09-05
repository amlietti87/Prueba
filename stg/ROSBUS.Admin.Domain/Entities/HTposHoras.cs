using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HTposHoras : Entity<string>
    {
        public HTposHoras()
        {
            HMediasVueltas = new HashSet<HMediasVueltas>();
            HMinxtipo = new HashSet<HMinxtipo>();
        }

       // public string CodTpoHora { get; set; }
        public string DscTpoHora { get; set; }
        public int Orden { get; set; }
        public string CodTpoHorabsas { get; set; }

        public ICollection<HMediasVueltas> HMediasVueltas { get; set; }
        public ICollection<HMinxtipo> HMinxtipo { get; set; }
    }
}
