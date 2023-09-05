using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspTopesFrancosGruposInspectores : Entity<int>
    {

        public long? GrupoInspectoresId { get; set; }
        public int? CantidadDiasMes { get; set; }
        public int? TopeFrancoInpsector { get; set; }
        public int? TopeFrancoGrupoInspector { get; set; }

        public InspGruposInspectores GrupoInspectores { get; set; }

    }
}
