using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoColisionFilter : FilterPagedListFullAudited<SinTipoColision, int>
    {
        public string Descripcion { get; set; }
        public bool? Anulado { get; set; }

        public override Expression<Func<SinTipoColision, bool>> GetFilterExpression()
        {
            Expression<Func<SinTipoColision, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinTipoColision, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == this.FilterText;
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinTipoColision, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
