using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class HNovxchof : Entity<string>
    {
        public int CodNov { get; set; }
        public DateTime FecDesde { get; set; }
        public DateTime? FecHasta { get; set; }
        public string Comenta { get; set; }
        public decimal? Anio { get; set; }
        public string VacPagas { get; set; }
        public decimal? Imprimio { get; set; }
        public string PasadaSueldos { get; set; }
        public string ProgAutomatica { get; set; }
    }
}
