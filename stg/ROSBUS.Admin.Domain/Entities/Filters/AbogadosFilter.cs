using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class AbogadosFilter : FilterPagedListFullAudited<SinAbogados, int>
    {
        public string ApellidoNombre { get; set; }
        public bool? Anulado { get; set; }
        public int? AbogadoId { get; set; }
        public override Expression<Func<SinAbogados, bool>> GetFilterExpression()
        {
            Expression<Func<SinAbogados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinAbogados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.ApellidoNombre.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinAbogados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (AbogadoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.AbogadoId.Value);
            }

            return Exp;
        }



    }
}
