using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class InspPlanillaIncognitosDetalle : Entity<int>
    {
        public int PlanillaIncognitoId { get; set; }
        public int PreguntaIncognitoId { get; set; }
        public virtual AppInspectores.InspPreguntasIncognitos PreguntaIncognitos { get; set; }
        public int? RespuestaIncognitoId { get; set; }
        public virtual AppInspectores.InspRespuestasIncognitos RespuestaIncognitos { get; set; }
        public string Observacion { get; set; }
        public virtual InspPlanillaIncognitos PlanillaIncognito { get; set; }
    }
}
