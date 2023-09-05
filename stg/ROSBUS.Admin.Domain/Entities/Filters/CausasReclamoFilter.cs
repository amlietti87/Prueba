using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.ART.Domain.Entities.Filters
{
    public class CausasReclamoFilter : FilterPagedListAudited<CausasReclamo, int>
    {
        public bool? Anulado { get; set; }

        public int? CausaReclamoId { get; set; }

        public override Expression<Func<CausasReclamo, bool>> GetFilterExpression()
        {
            Expression<Func<CausasReclamo, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<CausasReclamo, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<CausasReclamo, bool>> filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (CausaReclamoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.CausaReclamoId.Value);
            }

            return Exp;
        }
    }
}
