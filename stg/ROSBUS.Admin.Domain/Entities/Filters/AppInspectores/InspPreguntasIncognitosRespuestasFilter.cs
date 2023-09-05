using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspPreguntasIncognitosRespuestasFilter : FilterPagedListBase<InspPreguntasIncognitosRespuestas, int>
    {
        public int? PreguntaIncognitoId { get; set; }
        public override List<Expression<Func<InspPreguntasIncognitosRespuestas, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<InspPreguntasIncognitosRespuestas, object>>>
            {
                e=> e.Respuesta,
            };
        }

        public override Expression<Func<InspPreguntasIncognitosRespuestas, bool>> GetFilterExpression()
        {
            Expression<Func<InspPreguntasIncognitosRespuestas, bool>> Exp = base.GetFilterExpression();

            if (PreguntaIncognitoId.HasValue)
            {
                Exp = Exp.And(e => e.PreguntaIncognitoId == this.PreguntaIncognitoId.Value);
            }

            return Exp;
        }
    }
}