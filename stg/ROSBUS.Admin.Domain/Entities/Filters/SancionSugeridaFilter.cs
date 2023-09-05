using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SancionSugeridaFilter : FilterPagedListFullAudited<SinSancionSugerida, int>
    {
        public bool? Anulado { get; set; }

        public int? SancionId { get; set; }
        public override Expression<Func<SinSancionSugerida, bool>> GetFilterExpression()
        {
            Expression<Func<SinSancionSugerida, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinSancionSugerida, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinSancionSugerida, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }


            if (SancionId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.SancionId.Value);
            }
            return Exp;
        }
        


    }
}
