using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SubGalponDto :EntityDto<Decimal>
    {
        public SubGalponDto()
        {
            this.Configu = new List<ConfiguDto>();
        }
        public string DesSubg { get; set; }
        public DateTime? FecBaja { get; set; }
        public string Balanceo { get; set; }
        public string FltComodines { get; set; }
        public List<ConfiguDto> Configu { get; set; }
        public override string Description => this.DesSubg;
    }
}
