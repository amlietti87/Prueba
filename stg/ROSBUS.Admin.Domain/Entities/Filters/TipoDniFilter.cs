using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoDniFilter : FilterPagedListBase<TipoDni, int>
    {
        public string Descripcion { get; set; }

        public int? TipoDocId { get; set; }
        public bool? Anulado { get; set; }
        public override Expression<Func<TipoDni, bool>> GetFilterExpression()
        {
            Expression<Func<TipoDni, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<TipoDni, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == FilterText;
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<TipoDni, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }


            if (this.TipoDocId.HasValue)
            {
                Expression<Func<TipoDni, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Id == TipoDocId;
                Exp = Exp.Or(filterTextExp);
            }


            return Exp;
        }



    }
}
