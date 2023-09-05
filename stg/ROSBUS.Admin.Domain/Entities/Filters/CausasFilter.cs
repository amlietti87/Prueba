using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CausasFilter : FilterPagedListFullAudited<SinCausas, int>
    {
        public bool? Anulado { get; set; }
        public int? CausaId { get; set; }
        public override Expression<Func<SinCausas, bool>> GetFilterExpression()
        {
            Expression<Func<SinCausas, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinCausas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }


            if (Anulado.HasValue)
            {
                Expression<Func<SinCausas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (CausaId.HasValue) {
                Exp = Exp.Or(e => e.Id == this.CausaId.Value);
            }


            return Exp;
        }



    }
}
