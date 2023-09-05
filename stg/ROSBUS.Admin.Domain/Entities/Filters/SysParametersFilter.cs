using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SysParametersFilter : FilterPagedListBase<SysParameters, long>
    {
        public string Token { get; set; }

        public override List<Expression<Func<SysParameters, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SysParameters, object>>>
            {
                e=> e.SysDataType
            };
        }

        public override Expression<Func<SysParameters, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();

            if (!string.IsNullOrEmpty(this.Token))
            {
                exp = exp.And(e => e.Token == Token);
            }

            return exp;
        }

    }
}
