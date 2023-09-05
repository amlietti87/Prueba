using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class DetalleLesionDto :EntityDto<int>
    {
        public int InvolucradoId { get; set; }
        public string LugarAtencion { get; set; }
        public DateTime? FechaFactura { get; set; }
        public string Observaciones { get; set; }

        public string NroFactura { get; set; }
        public Decimal? ImporteFactura { get; set; }

        public override string Description => Observaciones;

    }
}
