using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{ 
    public partial class HKilometros
    {
        public int CodBan { get; set; }
        public int CodSec { get; set; }
        public decimal Kmr { get; set; }
        public decimal Km { get; set; }
        public int? CodBanderaColor { get; set; }
        public int? CodBanderaTup { get; set; }        
    }

}
