using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTipoViaje: Entity<int>
    {
        public string TravelMode { get; set; }
        public string Descripcion { get; set; }
        public ICollection<PlaPuntos> PlaPuntos { get; set; }

        public PlaTipoViaje()
        {
            PlaPuntos = new HashSet<PlaPuntos>();
        }
    }
}
