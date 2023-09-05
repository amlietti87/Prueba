using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CroElemenetoFilter : FilterPagedListBase<CroElemeneto, Guid>
    {


        public override Expression<Func<CroElemeneto, bool>> GetFilterExpression()
        {
            Expression<Func<CroElemeneto, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<CroElemeneto, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText) || e.Nombre.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }

        public override List<Expression<Func<CroElemeneto, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<CroElemeneto, object>>>
            {
                e=> e.Tipo,
                e=> e.TipoElemento
            };
        }


    }
}
