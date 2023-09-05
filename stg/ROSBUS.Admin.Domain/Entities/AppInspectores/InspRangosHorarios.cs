using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspRangosHorarios : AuditedEntity<int>
    {
        public InspRangosHorarios()
        {
            HFrancos = new HashSet<HFrancos>();
            InspGrupoInspectoresRangosHorarios = new HashSet<InspGrupoInspectoresRangosHorarios>();
            PersJornadasTrabajadas = new HashSet<PersJornadasTrabajadas>();
        }
        
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public bool EsFranco { get; set; }
        public bool EsFrancoTrabajado { get; set; }
        public int? NovedadId { get; set; }
        public DateTime? HoraDesde { get; set; }
        public DateTime? HoraHasta { get; set; }
        public string Color { get; set; }

        public ICollection<HFrancos> HFrancos { get; set; }
        public ICollection<InspGrupoInspectoresRangosHorarios> InspGrupoInspectoresRangosHorarios { get; set; }
        public ICollection<PersJornadasTrabajadas> PersJornadasTrabajadas { get; set; }
    }
}
