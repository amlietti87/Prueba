using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaLineaLineaHorariaDto: EntityDto<int>
    {

        public int? PlaLineaId { get; set; }
        public decimal? LineaId { get; set; }

        public String DescripcionLinea { get; set; }
        public string DescripcionPlaLinea { get; set; }

        public override string Description => DescripcionPlaLinea?.Trim();
    }
}
