using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaDistribucionDeCochesPorTipoDeDiaFilter : FilterPagedListBase<PlaDistribucionDeCochesPorTipoDeDia, int>
    {
        public int? CodHfecha { get; set; }        
        public int CodTdia { get; set; }
        public string PlanillaId { get; set; }

        public Decimal? cod_lin { get; set; }



    }
}
