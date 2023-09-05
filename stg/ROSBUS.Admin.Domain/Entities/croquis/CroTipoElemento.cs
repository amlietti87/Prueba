using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class CroTipoElemento : TECSO.FWK.Domain.Entities.Entity<int>
    {
        public CroTipoElemento()
        {
            CroElemeneto = new HashSet<CroElemeneto>();
        }
        //public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<CroElemeneto> CroElemeneto { get; set; }

    }
}
