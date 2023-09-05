using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class EstadosFilter : FilterPagedListAudited<ArtEstados, int>
    {
        public bool? Anulado { get; set; }
        public int? EstadoId { get; set; }
        public override Expression<Func<ArtEstados, bool>> GetFilterExpression()
        {
            Expression<Func<ArtEstados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (EstadoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.EstadoId.Value);
            }

            return Exp;
        }

    }
}
