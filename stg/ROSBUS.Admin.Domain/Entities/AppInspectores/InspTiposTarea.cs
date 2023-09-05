using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspTiposTarea : FullAuditedEntity<int>
    {
        public InspTiposTarea()
        {
            //InspGrupoInspectoresTiposTarea = new HashSet<InspGrupoInspectoresTarea>();
        }

        public string Descripcion { get; set; }
        
        //public ICollection<InspGrupoInspectoresTarea> InspGrupoInspectoresTiposTarea { get; set; }
    }
}
