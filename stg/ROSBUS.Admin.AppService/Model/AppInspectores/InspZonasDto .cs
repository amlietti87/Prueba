using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspZonasDto : AuditedEntityDto<int>
    {
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public bool Anulado { get; set; }

        public override string Description => this.Descripcion;
    }
}
