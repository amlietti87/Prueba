using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HProcMin 
    {
        public int CodMvuelta { get; set; }
        public int CodHsector { get; set; }
        public decimal Minuto { get; set; }

        public PlaCoordenadas CodHsectorNavigation { get; set; }
        public HMediasVueltas CodMvueltaNavigation { get; set; }
    }
}
