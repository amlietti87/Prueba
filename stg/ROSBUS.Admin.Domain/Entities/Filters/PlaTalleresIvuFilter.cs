using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaTalleresIvuFilter : FilterPagedListBase<PlaTalleresIvu, int>
    {

        public override Expression<Func<PlaTalleresIvu, bool>> GetFilterExpression()
        {
            Expression<Func<PlaTalleresIvu, bool>> Exp = base.GetFilterExpression();


            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<PlaTalleresIvu, bool>> filterTextExp = e => e.CodGalNavigation.DesGal.Contains(this.FilterText) || e.CodGalIvu.ToString() == this.FilterText;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }
        


    }
}
