using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoBanderaFilter : FilterPagedListBase<PlaTipoBandera, int>
    {
        public int? TipoBanderaId { get; set; }

        public override Expression<Func<PlaTipoBandera, bool>> GetFilterExpression()
        {
            Expression<Func<PlaTipoBandera, bool>> Exp = base.GetFilterExpression();


            if (TipoBanderaId.HasValue)
            {
                Exp = Exp.And(e => e.Id == this.TipoBanderaId.Value);
            }


            return Exp;
        }
    }
}
