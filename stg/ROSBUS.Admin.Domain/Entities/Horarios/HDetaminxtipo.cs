using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HDetaminxtipo
    {
        public int CodMinxtipo { get; set; }
        public decimal? Minuto { get; set; }
        public int CodHsector { get; set; }

        public PlaCoordenadas CodHsectorNavigation { get; set; }
        public HMinxtipo CodMinxtipoNavigation { get; set; }
    }
}
