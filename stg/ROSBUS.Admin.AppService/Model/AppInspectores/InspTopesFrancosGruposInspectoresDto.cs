using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspTopesFrancosGruposInspectoresDto: EntityDto<int>
    {
        public long? GrupoInspectoresId { get; set; }
        public int? CantidadDiasMes { get; set; }
        public int? TopeFrancoInpsector { get; set; }
        public int? TopeFrancoGrupoInspector { get; set; }

        public override string Description => this.Description;
    }
}
