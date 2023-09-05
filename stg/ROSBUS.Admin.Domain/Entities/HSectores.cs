using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HSectores
    {
        public int CodSec { get; set; }
        public int CodHsector { get; set; }
        public int CodSectorTarifario { get; set; }
        public int Orden { get; set; }
        public string VerEnResumen { get; set; }
        public bool? LlegaA { get; set; }
        public bool? SaleDe { get; set; }
        public PlaCoordenadas CodHsectorNavigation { get; set; }

        public BolSectoresTarifarios CodSectorTarifarioNavigation { get; set; }
    }
}
