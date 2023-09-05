using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class AdjuntosFilter : FilterPagedListBase<Adjuntos, Guid>
    {
        public string Nombre { get; set; }
        public List<Guid> Ids { get; set; }

        public override Expression<Func<Adjuntos, bool>> GetFilterExpression()
        {
            Expression<Func<Adjuntos, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Nombre))
            {
                Expression<Func<Adjuntos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Nombre == Nombre;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
