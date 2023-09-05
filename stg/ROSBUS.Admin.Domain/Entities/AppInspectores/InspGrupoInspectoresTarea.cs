using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspGrupoInspectoresTarea : Entity<int>
    {
        public long GrupoInspectoresId { get; set; }
        public int TareaId { get; set; }

        public InspGruposInspectores GrupoInspectores { get; set; }
        public InspTarea Tarea { get; set; }
    }
}
