using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaMinutosPorSectorFilter : FilterPagedListBase<PlaMinutosPorSector, long>
    {
        public DateTime Fecha { get; set; }
        public long IdSector { get; set; }

        public override Expression<Func<PlaMinutosPorSector, bool>> GetFilterExpression()
        {
            return e => e.Fecha.Date == this.Fecha && e.IdSector == this.IdSector;
        }
    }
}
