using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class MotivoInfraDto :EntityDto<String>
    {
        public string DesMotivo { get; set; }
        public decimal? Tipo { get; set; }
        public string TpoEnfoque { get; set; }
        public int? PtosEvaluacion { get; set; }
        public string CodMotivoBa { get; set; }

        public override string Description => this.DesMotivo;
    }
}
