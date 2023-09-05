using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTipoLinea : Entity<int>
    {
        public PlaTipoLinea()
        {
            PlaLineas = new HashSet<PlaLinea>();
        }
        
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int CantidadConductoresPorServicio { get; set; }

        public ICollection<PlaLinea> PlaLineas { get; set; }

    }
}
