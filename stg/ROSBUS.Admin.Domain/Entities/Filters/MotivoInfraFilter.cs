using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class MotivoInfraFilter : FilterPagedListBase<MotivoInfra, string>
    {

        public override Func<MotivoInfra, ItemDto<string>> GetItmDTO()
        {
            return e => new ItemDto<string>(e.Id, e.DesMotivo);
        }

    }
}
