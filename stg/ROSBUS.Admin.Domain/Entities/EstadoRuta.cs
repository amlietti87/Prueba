using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaEstadoRuta : Entity<int>
    {
        public PlaEstadoRuta()
        {
            Rutas = new HashSet<GpsRecorridos>();
        }
        
        public string Nombre { get; set; }

       public ICollection<GpsRecorridos> Rutas { get; set; }
    }
}
