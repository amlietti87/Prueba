using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class Novedades : Entity<int>
    {
        public string DesNov { get; set; }
        public string HorSue { get; set; }
        public string Variable { get; set; }
        public string PorHora { get; set; }
        public string FranNov { get; set; }
        public string AbrNov { get; set; }
        public string FecHastaEditable { get; set; }
        public string ClaseAusensia { get; set; }
    }
}
