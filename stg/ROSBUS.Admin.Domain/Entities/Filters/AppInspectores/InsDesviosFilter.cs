using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InsDesviosFilter: FilterPagedListFullAudited<InsDesvios, long>
    {
        public int? unidadNegocio { get; set; }

        public override Expression<Func<InsDesvios, bool>> GetFilterExpression()
        {
            Expression<Func<InsDesvios, bool>> Exp = base.GetFilterExpression();
           
                Expression<Func<InsDesvios, bool>> filterTextExp = e => true;
                filterTextExp = e => e.IsDeleted == false;
                Exp = Exp.And(filterTextExp);

            if (unidadNegocio.HasValue)
            {
                Exp = Exp.Or(e => e.SucursalId == this.unidadNegocio.Value);
            }

            return base.GetFilterExpression();

        }
    }

}
