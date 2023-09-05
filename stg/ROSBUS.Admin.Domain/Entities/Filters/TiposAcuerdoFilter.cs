using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.ART.Domain.Entities.Filters
{
    public class TiposAcuerdoFilter : FilterPagedListAudited<TiposAcuerdo, int>
    {
        public bool? Anulado { get; set; }

        public int? TipoAcuerdoId { get; set; }

        public override Expression<Func<TiposAcuerdo, bool>> GetFilterExpression()
        {
            Expression<Func<TiposAcuerdo, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<TiposAcuerdo, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<TiposAcuerdo, bool>> filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (TipoAcuerdoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.TipoAcuerdoId.Value);
            }

            return Exp;
        }
    }
}
