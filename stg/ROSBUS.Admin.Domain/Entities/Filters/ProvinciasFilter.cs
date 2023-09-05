using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class ProvinciasFilter : FilterPagedListBase<Provincias, int>
    {
        public string Dsc_provincia { get; set; }

        public override Expression<Func<Provincias, bool>> GetFilterExpression()
        {
            Expression<Func<Provincias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Dsc_provincia))
            {
                Expression<Func<Provincias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.DscProvincia == Dsc_provincia;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
