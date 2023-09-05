using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspTiposTareaFilter : FilterPagedListFullAudited<InspTiposTarea, int>
    {
        public override Expression<Func<InspTiposTarea, bool>> GetFilterExpression()
        {
            Expression<Func<InspTiposTarea, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<InspTiposTarea, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                baseFE = baseFE.And(filterTextExp);
            }

            return baseFE;

        }
    }
}
