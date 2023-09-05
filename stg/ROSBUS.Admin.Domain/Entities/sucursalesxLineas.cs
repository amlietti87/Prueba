using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SucursalesxLineas : Entity<int>
    {
        //public int CodSucursal { get; set; }
        public decimal CodLinea { get; set; }
        public DateTime? FecBaja { get; set; }
        public Linea Lineas { get; set; }

    }
}
