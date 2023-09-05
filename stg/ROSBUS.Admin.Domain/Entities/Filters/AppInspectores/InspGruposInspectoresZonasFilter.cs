using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspGruposInspectoresZonasFilter : FilterPagedListBase<InspGrupoInspectoresZona, int>
    {
        public int? GrupoinspectoresId { get; set; }
        public override List<Expression<Func<InspGrupoInspectoresZona, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<InspGrupoInspectoresZona, object>>>
            {
                e=> e.Zona,
            };
        }

        public override Expression<Func<InspGrupoInspectoresZona, bool>> GetFilterExpression()
        {
            Expression<Func<InspGrupoInspectoresZona, bool>> Exp = base.GetFilterExpression();

            if (GrupoinspectoresId.HasValue)
            {
                Exp = Exp.And(e => e.GrupoInspectoresId == this.GrupoinspectoresId.Value);
            }

            return Exp;
        }
    }
}