using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class DetalleLesionFilter : FilterPagedListBase<SinDetalleLesion, int>
    {
        public string Observaciones { get; set; }

        public override Expression<Func<SinDetalleLesion, bool>> GetFilterExpression()
        {
            Expression<Func<SinDetalleLesion, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Observaciones))
            {
                Expression<Func<SinDetalleLesion, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Observaciones == Observaciones;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
