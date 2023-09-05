using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspGruposInspectoresDto : AuditedEntityDto<long>
    {
        public InspGruposInspectoresDto()
        {
            InspGrupoInspectoresRangosHorarios = new List<InspGrupoInspectoresRangosHorariosDto>(); 
            InspGrupoInspectoresZona = new List<InspGrupoInspectoresZonasDto>();
            InspGrupoInspectoresTareas = new List<InspGrupoInspectoresTareaDto>();
            InspGrupoInspectoresTurnos = new List<InspGruposInspectoresTurnoDto>();

        }

        public string Descripcion { get; set; }
        public int? NotificacionId { get; set; }
        public int? LineaId { get; set; }
        public bool Anulado { get; set; }

        public NotificationDto Notificacion { get; set; }
        public LineaDto Linea { get; set; }
        public List<InspGrupoInspectoresRangosHorariosDto> InspGrupoInspectoresRangosHorarios { get; set; }
        public List<InspGrupoInspectoresZonasDto> InspGrupoInspectoresZona { get; set; }
        public List<InspGrupoInspectoresTareaDto> InspGrupoInspectoresTareas { get; set; }

        public List<InspGruposInspectoresTurnoDto> InspGrupoInspectoresTurnos { get; set; }

        public override string Description => this.Descripcion;
    }
}
