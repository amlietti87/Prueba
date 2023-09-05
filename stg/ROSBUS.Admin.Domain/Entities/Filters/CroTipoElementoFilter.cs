using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CroTipoElementoFilter : FilterPagedListBase<CroTipoElemento, int>
    {

        public override Expression<Func<CroTipoElemento, bool>> GetFilterExpression()
        {
            Expression<Func<CroTipoElemento, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<CroTipoElemento, bool>> filterTextExp = e =>  e.Nombre.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }


        public override List<Expression<Func<CroTipoElemento, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<CroTipoElemento, object>>>
            {
                e=> e.CroElemeneto
            };
        }

    }
}
