using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class ReclamoCuotasFilter : FilterPagedListFullAudited<SinReclamoCuotas, int>
    {
        public string Concepto { get; set; }

        public override Expression<Func<SinReclamoCuotas, bool>> GetFilterExpression()
        {
            Expression<Func<SinReclamoCuotas, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Concepto))
            {
                Expression<Func<SinReclamoCuotas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Concepto == Concepto;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
