using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspGruposInspectoresFilter : FilterPagedListAudited<InspGruposInspectores, long>
    {
        public int? Anulado { get; set; }
        public override Expression<Func<InspGruposInspectores, bool>> GetFilterExpression()
        {
            Expression<Func<InspGruposInspectores, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<InspGruposInspectores, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                baseFE = baseFE.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                if (Anulado.Value == 2)
                {
                    baseFE = baseFE.And(e => e.Anulado == false);
                }

                if (Anulado.Value == 1)
                {
                    baseFE = baseFE.And(e => e.Anulado == true);
                }
            }


            return baseFE;


        }

    }
}