using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspDiagramasInspectoresTurnosDto : EntityDto<int>
    {
        public InspDiagramasInspectoresTurnosDto()
        {

        }

        public int DiagramaInspectoresId { get; set; }
        public int TurnoId { get; set; }

        public PersTurnosDto Turno { get; set; }

        public override string Description => "";
    }
}
