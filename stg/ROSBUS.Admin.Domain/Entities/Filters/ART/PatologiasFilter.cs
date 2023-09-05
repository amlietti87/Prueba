using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class PatologiasFilter : FilterPagedListAudited<ArtPatologias, int>
    {
        public bool? Anulado { get; set; }
        public int? PatologiaId { get; set; }
        public override Expression<Func<ArtPatologias, bool>> GetFilterExpression()
        {
            Expression<Func<ArtPatologias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtPatologias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtPatologias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (PatologiaId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.PatologiaId.Value);
            }

            return Exp;
        }

    }
}
