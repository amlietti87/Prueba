using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspPreguntasIncognitosRespuestas : Entity<int>
    {
        public int PreguntaIncognitoId { get; set; }
        public int? Orden { get; set; }
        public int RespuestaId { get; set; }
        public InspPreguntasIncognitos PreguntaIncognito { get; set; }
        public InspRespuestasIncognitos Respuesta { get; set; }
    }
}
