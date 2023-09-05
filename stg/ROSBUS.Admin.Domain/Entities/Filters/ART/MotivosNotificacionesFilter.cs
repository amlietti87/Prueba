using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class MotivosNotificacionesFilter : FilterPagedListAudited<ArtMotivosNotificaciones, int>
    {
        public bool? Anulado { get; set; }
        public int? MotivoNotificacionId { get; set; }
        public override Expression<Func<ArtMotivosNotificaciones, bool>> GetFilterExpression()
        {
            Expression<Func<ArtMotivosNotificaciones, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtMotivosNotificaciones, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtMotivosNotificaciones, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (MotivoNotificacionId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.MotivoNotificacionId.Value);
            }

            return Exp;
        }
    }
}
