using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;

namespace ROSBUS.Admin.Domain.Entities
{
    public class InspTarea : AuditedEntity<int>
    {
        public InspTarea()
        {
            TareasCampos = new HashSet<InspTareaCampo>();
            GruposInspectoresTareas = new HashSet<InspGrupoInspectoresTarea>();
        }
        public string  Descripcion { get; set; }
        public bool Anulado { get; set; }
        public ICollection<InspTareaCampo> TareasCampos { get; set; }
        public ICollection<InspGrupoInspectoresTarea> GruposInspectoresTareas { get; set; }
    }
}
