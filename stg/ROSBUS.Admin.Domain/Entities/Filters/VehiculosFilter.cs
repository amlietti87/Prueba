using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class VehiculosFilter : FilterPagedListBase<SinVehiculos, int>
    {
        public string Modelo { get; set; }

        public override Expression<Func<SinVehiculos, bool>> GetFilterExpression()
        {
            Expression<Func<SinVehiculos, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Modelo))
            {
                Expression<Func<SinVehiculos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Modelo == Modelo;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
