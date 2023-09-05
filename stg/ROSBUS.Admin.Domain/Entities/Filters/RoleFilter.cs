using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class RoleFilter : FilterPagedListFullAudited<SysRoles, int>
    {


        public override Expression<Func<SysRoles, bool>> GetFilterExpression()
        {
            Expression<Func<SysRoles, bool>> Exp = base.GetFilterExpression();
             
            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SysRoles, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Name.Contains(this.FilterText) || e.DisplayName.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }
    }
}
