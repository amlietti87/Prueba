using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaMinutosPorSector : Entity<long>
    {
        public long IdSector { get; set; }
        public int IdBandaHoraria { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Demora { get; set; }

        public PlaBandasHorarias BandasHoraria { get; set; }
        public PlaSector Sector { get; set; }
    }
}
