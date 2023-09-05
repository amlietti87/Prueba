using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class VehiculosDto :EntityDto<int>
    {
        public int? SeguroId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Dominio { get; set; }
        public string Poliza { get; set; }

        public override string Description => string.Empty;

    }
}
