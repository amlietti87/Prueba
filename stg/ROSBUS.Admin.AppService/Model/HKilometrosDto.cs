using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HKilometrosDto : EntityDto<int>
    { 
        public override string Description => string.Empty;

        public int CodBan { get; set; }
        public int CodSec { get; set; }
        public decimal Kmr { get; set; }
        public decimal Km { get; set; }
        public int? CodBanderaColor { get; set; }
        public int? CodBanderaTup { get; set; }

    }
}
