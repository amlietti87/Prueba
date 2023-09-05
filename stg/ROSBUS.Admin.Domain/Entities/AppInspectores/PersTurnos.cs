using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class PersTurnos : Entity<int>
    {
        public PersTurnos()
        {
            InspDiagramaInspectoresTurnos = new HashSet<InspDiagramasInspectoresTurnos>();
            InspGruposInspectoresTurnos = new HashSet<InspGruposInspectoresTurnos>();
            //SysUsersAd = new HashSet<SysUsersAd>();
        }
        
        public string DscTurno { get; set; }
        public int Orden { get; set; }
        public string Color { get; set; }

        public ICollection<InspDiagramasInspectoresTurnos> InspDiagramaInspectoresTurnos { get; set; }
        public ICollection<InspGruposInspectoresTurnos> InspGruposInspectoresTurnos { get; set; }
        //public ICollection<SysUsersAd> SysUsersAd { get; set; }
    }
}
