using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoDanioFilter : FilterPagedListBase<SinTipoDanio, int>
    {
        public string Descripcion { get; set; }

        public override Expression<Func<SinTipoDanio, bool>> GetFilterExpression()
        {
            Expression<Func<SinTipoDanio, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinTipoDanio, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == this.FilterText;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
