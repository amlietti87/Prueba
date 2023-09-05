using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspGruposInspectoresTurnoDto : EntityDto<int>
    {
        public long GrupoInspectoresId { get; set; }
        public int  TurnoId { get; set; }
        public string TurnoNombre { get; set; }
        
        //public InspTurnos Turno { get; set; }

        public override string Description => this.ToString();
    }
}
