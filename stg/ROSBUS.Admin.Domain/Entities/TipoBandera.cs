using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTipoBandera : Entity<int>
    {
        public PlaTipoBandera()
        {
            //Bandera = new HashSet<Bandera>();
        }

        public string Nombre { get; set; }

        //public ICollection<Bandera> Bandera { get; set; }
    }
}
