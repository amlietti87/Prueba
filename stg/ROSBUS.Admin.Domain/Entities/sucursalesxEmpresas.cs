using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SucursalesxEmpresas : Entity<int>
    {
        //public int CodSucursal { get; set; }
        public decimal CodEmpr { get; set; }
        public DateTime? FecBaja { get; set; }

        //public Sucursales CodSucursalNavigation { get; set; }

    }
}
