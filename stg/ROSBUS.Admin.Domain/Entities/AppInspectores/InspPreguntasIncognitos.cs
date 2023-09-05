using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspPreguntasIncognitos : AuditedEntity<int>
    {
        public InspPreguntasIncognitos()
        {
            InspPreguntasIncognitosRespuestas = new HashSet<InspPreguntasIncognitosRespuestas>();
            InspPlanillaIncognitosDetalles = new HashSet<InspPlanillaIncognitosDetalle>();
        }

        public string Descripcion { get; set; }

        public bool RespuestaRequerida { get; set; }
        public bool MostrarObservacion { get; set; }
        public int? Orden { get; set; }
        public bool Anulado { get; set; }

        public ICollection<InspPreguntasIncognitosRespuestas> InspPreguntasIncognitosRespuestas { get; set; }
        public ICollection<InspPlanillaIncognitosDetalle> InspPlanillaIncognitosDetalles { get; set; }

    }
}
