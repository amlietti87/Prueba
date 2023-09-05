using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlanCamDto :EntityDto<decimal>
    {
        public string DesPlan { get; set; }
        public string Depot { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Fecha { get; set; }
        public override string Description => this.DesPlan;
    }
}
