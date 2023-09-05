using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class PersTurnosFilter : FilterPagedListBase<PersTurnos, int>
    {
        public int? Anulado { get; set; }

        public int? GrupoInspectorId { get; set; }
        public override Expression<Func<PersTurnos, bool>> GetFilterExpression()
        {
            Expression<Func<PersTurnos, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<PersTurnos, bool>> filterTextExp = e => e.DscTurno.Contains(this.FilterText);
                baseFE = baseFE.And(filterTextExp);
            }

            if (GrupoInspectorId.HasValue)
            {
                Expression<Func<PersTurnos, bool>> filterTextExp = e => e.InspGruposInspectoresTurnos.Any(g => g.GrupoInspectoresId == this.GrupoInspectorId);
                baseFE = baseFE.And(filterTextExp);
            }


            return baseFE;


        }

        public override List<Expression<Func<PersTurnos, object>>> GetIncludesForGetById()
        {
            var include = base.GetIncludesForGetById();

            include.Add(e => e.InspGruposInspectoresTurnos);

            return include;
        }
    }
}
