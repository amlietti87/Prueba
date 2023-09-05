using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspDiagramasInspectoresTurnos : Entity<int>
    {     
        
        public int DiagramaInspectoresId { get; set; }
        public int TurnoId { get; set; }

        public InspDiagramasInspectores DiagramaInspectores { get; set; }
        public PersTurnos Turno { get; set; }
    }
}

