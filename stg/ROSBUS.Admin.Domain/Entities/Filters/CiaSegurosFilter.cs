using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CiaSegurosFilter : FilterPagedListFullAudited<SinCiaSeguros, int>
    {
        public string Descripcion { get; set; }
        public bool? Anulado { get; set; }

        public int? SeguroId { get; set; }
        public override Expression<Func<SinCiaSeguros, bool>> GetFilterExpression()
        {
            Expression<Func<SinCiaSeguros, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinCiaSeguros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText) || e.Encargado.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinCiaSeguros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (SeguroId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.SeguroId.Value);
            }

            return Exp;
        }



    }
}
