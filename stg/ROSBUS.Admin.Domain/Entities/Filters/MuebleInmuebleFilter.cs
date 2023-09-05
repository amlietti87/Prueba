using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class MuebleInmuebleFilter : FilterPagedListBase<SinMuebleInmueble, int>
    {
        public string Lugar { get; set; }

        public override Expression<Func<SinMuebleInmueble, bool>> GetFilterExpression()
        {
            Expression<Func<SinMuebleInmueble, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Lugar))
            {
                Expression<Func<SinMuebleInmueble, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Lugar == Lugar;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
