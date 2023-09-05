using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class PrestadoresMedicosFilter : FilterPagedListAudited<ArtPrestadoresMedicos, int>
    {

        public bool? Anulado { get; set; }
        public int? PrestadorMedicoId { get; set; }
        public override Expression<Func<ArtPrestadoresMedicos, bool>> GetFilterExpression()
        {
            Expression<Func<ArtPrestadoresMedicos, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtPrestadoresMedicos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<ArtPrestadoresMedicos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (PrestadorMedicoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.PrestadorMedicoId.Value);
            }

            return Exp;
        }

    }
}
