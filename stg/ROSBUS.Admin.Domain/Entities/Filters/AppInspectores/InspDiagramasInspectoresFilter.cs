using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    

    public class InspDiagramasInspectoresFilter : FilterPagedListFullAudited<InspDiagramasInspectores, int>
    {
        public int? Mes { get; set; }

        public int? Anio { get; set; }

        public int? GruposInspectores { get; set; }

        public int? EstadoDiagrama { get; set; }

        public override Expression<Func<InspDiagramasInspectores, bool>> GetFilterExpression()
        {
            Expression<Func<InspDiagramasInspectores, bool>> baseFE = base.GetFilterExpression();

            if (Mes.HasValue)
            {
               baseFE = baseFE.And(e => e.Mes == Mes);                          
            }

            if (Anio.HasValue)
            {
                baseFE = baseFE.And(e => e.Anio == Anio);
            }

            if(GruposInspectores.HasValue)
            {
                baseFE = baseFE.And(e => e.GrupoInspectoresId == GruposInspectores);
            }

            if (EstadoDiagrama.HasValue)
            {
                baseFE = baseFE.And(e => e.EstadoDiagramaId == EstadoDiagrama);
            }

            return baseFE;
        }
    }
}
