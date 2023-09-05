using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.ART.Domain.Entities.Filters
{
    public class RubrosSalarialesFilter : FilterPagedListAudited<RubrosSalariales, int>
    {
        public bool? Anulado { get; set; }

        public int? RubroSalarialId { get; set; }

        public override Expression<Func<RubrosSalariales, bool>> GetFilterExpression()
        {
            Expression<Func<RubrosSalariales, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<RubrosSalariales, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<RubrosSalariales, bool>> filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (RubroSalarialId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.RubroSalarialId.Value);
            }

            return Exp;
        }
    }
}
