using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaBandasHorarias : Entity<int>
    {
        public PlaBandasHorarias()
        {
            PlaMinutosPorSector = new HashSet<PlaMinutosPorSector>();
        }
        
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }

        public ICollection<PlaMinutosPorSector> PlaMinutosPorSector { get; set; }
    }
}
