using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class sucursalesFilter : FilterPagedListBase<Sucursales, int>
    {

        public override Func<Sucursales, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.DscSucursal);
        }
    }
}
