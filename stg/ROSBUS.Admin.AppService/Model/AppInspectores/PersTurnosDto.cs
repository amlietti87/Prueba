using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class PersTurnosDto : EntityDto<int>
    {
        public string DscTurno { get; set; }
        public int Orden { get; set; }
        public string Color { get; set; }

        public override string Description => this.DscTurno;
    }
}
