using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspGruposInspectores : AuditedEntity<long>
    {
        public InspGruposInspectores()
        {
            InspDiagramasInspectores = new HashSet<InspDiagramasInspectores>();
            InspGrupoInspectoresTareas = new HashSet<InspGrupoInspectoresTarea>();
            InspGrupoInspectoresRangosHorarios = new HashSet<InspGrupoInspectoresRangosHorarios>();
            InspGrupoInspectoresZona = new HashSet<InspGrupoInspectoresZona>();
            InspTopesFrancosGruposInspectores = new HashSet<InspTopesFrancosGruposInspectores>();
            InspTopesGruposInspectores = new HashSet<InspTopesGruposInspectores>();
            InspGruposInspectoresTurnos = new HashSet<InspGruposInspectoresTurnos>();
        }

        public string Descripcion { get; set; }
        public int? NotificacionId { get; set; }
        public decimal? LineaId { get; set; }
        public bool Anulado { get; set; }

        public Notification Notificacion { get; set; }
        public Linea Linea { get; set; }

        public ICollection<InspDiagramasInspectores> InspDiagramasInspectores { get; set; }
        public ICollection<InspGrupoInspectoresTarea> InspGrupoInspectoresTareas{ get; set; }
        public ICollection<InspGrupoInspectoresRangosHorarios> InspGrupoInspectoresRangosHorarios { get; set; }
        public ICollection<InspGrupoInspectoresZona> InspGrupoInspectoresZona { get; set; }
        public ICollection<InspTopesFrancosGruposInspectores> InspTopesFrancosGruposInspectores { get; set; }
        public ICollection<InspTopesGruposInspectores> InspTopesGruposInspectores { get; set; }
        public ICollection<InspGruposInspectoresTurnos> InspGruposInspectoresTurnos { get; set; }

    }
}
