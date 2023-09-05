using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspPreguntasIncognitosFilter : FilterPagedListAudited<InspPreguntasIncognitos, int>
    {
        public int? Anulado { get; set; }
        public override Expression<Func<InspPreguntasIncognitos, bool>> GetFilterExpression()
        {
            Expression<Func<InspPreguntasIncognitos, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<InspPreguntasIncognitos, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
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