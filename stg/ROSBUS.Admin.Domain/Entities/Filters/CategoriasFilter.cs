using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CategoriasFilter : FilterPagedListFullAudited<SinCategorias, int>
    {
        public int? ConsecuenciaId { get; set; }

        public override Expression<Func<SinCategorias, bool>> GetFilterExpression()
        {
            Expression<Func<SinCategorias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinCategorias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }
            if (ConsecuenciaId.HasValue && ConsecuenciaId.Value != 0)
            {
                Expression<Func<SinCategorias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.ConsecuenciaId == this.ConsecuenciaId.Value && e.Anulado == false;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
