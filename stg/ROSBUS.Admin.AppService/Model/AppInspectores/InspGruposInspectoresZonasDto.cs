using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspGrupoInspectoresZonasDto : EntityDto<int>
    {

        public long GrupoInspectoresId { get; set; }
        public int ZonaId { get; set; }

        //public InspGruposInspectoresDto GrupoInspectores { get; set; }
        //public InspZonasDto Zona { get; set; }

        public string ZonaNombre { get; set; }


        public override string Description => this.ToString();
    }
}
