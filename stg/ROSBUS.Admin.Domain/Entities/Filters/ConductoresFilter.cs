using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class ConductoresFilter : FilterPagedListBase<SinConductores, int>
    {

        public override Expression<Func<SinConductores, bool>> GetFilterExpression()
        {
            Expression<Func<SinConductores, bool>> Exp = base.GetFilterExpression();


            return Exp;
        }



    }
}
