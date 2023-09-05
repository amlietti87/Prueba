using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspGruposInspectoresTurnosFilter : FilterPagedListBase<InspGruposInspectoresTurnos, int>
    {
        public int? GrupoInspectorId { get; set; }
        public override Expression<Func<InspGruposInspectoresTurnos, bool>> GetFilterExpression()
        {
            Expression<Func<InspGruposInspectoresTurnos, bool>> baseFE = base.GetFilterExpression();


            if (GrupoInspectorId.HasValue)
            {
               baseFE = baseFE.And(e => e.GrupoInspectoresId == GrupoInspectorId);                           
            }

            return baseFE;


        }
    }
}