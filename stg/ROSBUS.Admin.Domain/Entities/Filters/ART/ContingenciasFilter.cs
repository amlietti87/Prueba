using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class ContingenciasFilter : FilterPagedListAudited<ArtContingencias, int>
    {

        public bool? Anulado { get; set; }
        public int? ContingenciaId { get; set; }
        public override Expression<Func<ArtContingencias, bool>> GetFilterExpression()
        {
            Expression<Func<ArtContingencias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtContingencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtContingencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (ContingenciaId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.ContingenciaId.Value);
            }

            return Exp;
        }

    }
}
