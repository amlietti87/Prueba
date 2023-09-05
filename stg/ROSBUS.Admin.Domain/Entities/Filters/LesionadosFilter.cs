using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class LesionadosFilter : FilterPagedListBase<SinLesionados, int>
    {
        public string LugarAtencion { get; set; }

        public override Expression<Func<SinLesionados, bool>> GetFilterExpression()
        {
            Expression<Func<SinLesionados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.LugarAtencion))
            {
                Expression<Func<SinLesionados, bool>> filterTextExp = e => true;
                //filterTextExp = e => e.LugarAtencion == LugarAtencion;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
