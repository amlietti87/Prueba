using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class InspPlanillaIncognitosFilter : FilterPagedListBase<InspPlanillaIncognitos, int>
    {

        public override List<Expression<Func<InspPlanillaIncognitos, object>>> GetIncludesForGetById()
        {
            var query =  base.GetIncludesForGetById();

            query.Add(e => e.InspPlanillaIncognitosDetalle);
            return query;
        }

        public override List<Expression<Func<InspPlanillaIncognitos, object>>> GetIncludesForPageList()
        {

            var query = base.GetIncludesForPageList();

            query.Add(e => e.InspPlanillaIncognitosDetalle);
            return query;
        }

    }
}
