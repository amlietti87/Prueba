using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class MotivoInfra : Entity<string>
    {
        public string DesMotivo { get; set; }
        public decimal? Tipo { get; set; }
        public string TpoEnfoque { get; set; }
        public int? PtosEvaluacion { get; set; }
        public string CodMotivoBa { get; set; }
    }
}
