using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class ConductasNormasFilter : FilterPagedListFullAudited<SinConductasNormas, int>
    {
        public bool? Anulado { get; set; }

        public int? ConductaId { get; set; }

        public override Expression<Func<SinConductasNormas, bool>> GetFilterExpression()
        {
            Expression<Func<SinConductasNormas, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinConductasNormas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText.ToLower());
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinConductasNormas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (ConductaId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.ConductaId.Value);
            }


            return Exp;
        }



    }
}
