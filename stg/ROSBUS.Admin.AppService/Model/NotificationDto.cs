using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class NotificationDto: EntityDto<int>
    {
        public string Descripcion { get; set; }
        public string Token { get; set; }
        public string Permiso { get; set; }

        public override string Description => this.Descripcion;
    }
}
