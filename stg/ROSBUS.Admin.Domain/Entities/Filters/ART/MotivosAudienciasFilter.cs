using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class MotivosAudienciasFilter : FilterPagedListAudited<ArtMotivosAudiencias, int>
    {
        public bool? Anulado { get; set; }
        public int? MotivoAudienciaId { get; set; }
        public override Expression<Func<ArtMotivosAudiencias, bool>> GetFilterExpression()
        {
            Expression<Func<ArtMotivosAudiencias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtMotivosAudiencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtMotivosAudiencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (MotivoAudienciaId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.MotivoAudienciaId.Value);
            }

            return Exp;
        }
    }
}
