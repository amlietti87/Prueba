using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HFechas : Entity<int>
    {
        public DateTime Fecha { get; set; }
        public int CodTdia { get; set; }
        public string Feriado { get; set; }
        public string CompensatorioPago { get; set; }
    }
}
