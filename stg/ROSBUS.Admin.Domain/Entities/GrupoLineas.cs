using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaGrupoLineas : Entity<int>
    {
        public PlaGrupoLineas()
        {
            Linea = new HashSet<Linea>();
        }
        
        public string Nombre { get; set; }


        public int SucursalId { get; set; }
                
        public Sucursales Sucursal { get; set; }
        public ICollection<Linea> Linea { get; set; }
    }
}
