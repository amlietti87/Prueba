using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class TratamientosFilter : FilterPagedListBase<ArtTratamientos, int>
    {
        public bool? Anulado { get; set; }
        public int? TratamientoId { get; set; }
        public override Expression<Func<ArtTratamientos, bool>> GetFilterExpression()
        {
            Expression<Func<ArtTratamientos, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtTratamientos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtTratamientos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (TratamientoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.TratamientoId.Value);
            }

            return Exp;
        }
    }
}
