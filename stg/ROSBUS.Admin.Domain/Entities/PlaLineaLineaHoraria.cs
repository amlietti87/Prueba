using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaLineaLineaHoraria : Entity<int>
    {
        //public int Id { get; set; }
        public int? PlaLineaId { get; set; }
        public decimal? LineaId { get; set; }

        public Linea Linea { get; set; }
        public PlaLinea PlaLinea { get; set; }


    }
}
