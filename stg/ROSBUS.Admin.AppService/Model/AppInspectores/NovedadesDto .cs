using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class NovedadesDto : AuditedEntityDto<int>
    {
        public string DesNov { get; set; }
        public string HorSue { get; set; }
        public string Variable { get; set; }
        public string PorHora { get; set; }
        public string FranNov { get; set; }
        public string AbrNov { get; set; }
        public string FecHastaEditable { get; set; }
        public string ClaseAusensia { get; set; }

        public override string Description => this.DesNov;
    }
}
