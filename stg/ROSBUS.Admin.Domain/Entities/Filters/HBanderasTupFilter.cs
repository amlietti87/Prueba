using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HBanderasTupFilter : FilterPagedListBase<HBanderasTup, int>
    {

        public override Func<HBanderasTup, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.Descripcion);
        }
    }
}
