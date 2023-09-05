using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspDiagramasInspectoresTurnosFilter : FilterPagedListBase<InspDiagramasInspectoresTurnos, int>
    {
        public int? DiagrmasInspectoresId { get; set; }
        public override Expression<Func<InspDiagramasInspectoresTurnos, bool>> GetFilterExpression()
        {
            Expression<Func<InspDiagramasInspectoresTurnos, bool>> baseFE = base.GetFilterExpression();


            if (DiagrmasInspectoresId.HasValue)
            {
                baseFE = baseFE.And(e => e.DiagramaInspectoresId == DiagrmasInspectoresId);
            }

            return baseFE;


        }


    }
}
