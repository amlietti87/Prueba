using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SiniestrosConsecuenciasFilter : FilterPagedListBase<SinSiniestrosConsecuencias, int>
    {

        public override List<Expression<Func<SinSiniestrosConsecuencias, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinSiniestrosConsecuencias, object>>>
            {
                e=> e.Categoria,
                e=> e.Consecuencia,
            };
        }

    }
}
