using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspTiposTareaDto : AuditedEntityDto<int>
    {
        public string Descripcion { get; set; }     

        public override string Description => this.Descripcion;
    }
}
