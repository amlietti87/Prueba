using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SubCausasFilter : FilterPagedListFullAudited<SinSubCausas, int>
    {
        public int? CausaId { get; set; }
        public int? SubCausaId { get; set; }
        public override Expression<Func<SinSubCausas, bool>> GetFilterExpression()
        {
            Expression<Func<SinSubCausas, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinSubCausas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }
            if (CausaId.HasValue && CausaId.Value != 0)
            {
                Expression<Func<SinSubCausas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.CausaId == this.CausaId.Value && e.Anulado == false;
                Exp = Exp.And(filterTextExp);
            }
            if (SubCausaId.HasValue && SubCausaId.Value != 0)
            {
                Expression<Func<SinSubCausas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Id == this.SubCausaId.Value;
                Exp = Exp.Or(filterTextExp);
            }
            return Exp;
        }



    }
}
