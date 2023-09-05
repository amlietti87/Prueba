using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HTposHorasFilter : FilterPagedListBase<HTposHoras, string>
    {
        public override Func<HTposHoras, ItemDto<string>> GetItmDTO()
        {
            return e => new ItemDto<string>(e.Id, e.DscTpoHora);
        }
    }
}
