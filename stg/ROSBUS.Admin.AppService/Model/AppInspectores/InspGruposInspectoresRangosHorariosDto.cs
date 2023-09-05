using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspGrupoInspectoresRangosHorariosDto : EntityDto<int>
    {
        public long GrupoInspectoresId { get; set; }
        public int RangoHorarioId { get; set; }
        public string NombreRangoHorario { get; set; }
        
        //public InspTurnos Turno { get; set; }

        public override string Description => this.ToString();
    }
}
