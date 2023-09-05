using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ReclamoCuotasDto :EntityDto<int>
    {
        public int ReclamoId { get; set; }

        public DateTime? FechaVencimiento { get; set; }
        public decimal? Monto { get; set; }
        public string Concepto { get; set; }

        public override string Description => string.Empty;

    }
}
