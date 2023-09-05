using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspZonas : AuditedEntity<int>
    {
        public InspZonas()
        {
            InspGrupoInspectoresZona = new HashSet<InspGrupoInspectoresZona>();
            PersJornadasTrabajadas = new HashSet<PersJornadasTrabajadas>();
        }
        
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public bool Anulado { get; set; }

        public ICollection<InspGrupoInspectoresZona> InspGrupoInspectoresZona { get; set; }
        public ICollection<PersJornadasTrabajadas> PersJornadasTrabajadas { get; set; }
    }
}
