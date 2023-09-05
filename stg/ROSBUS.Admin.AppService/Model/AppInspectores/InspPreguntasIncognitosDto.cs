using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspPreguntasIncognitosDto : AuditedEntityDto<int>
    {
        public InspPreguntasIncognitosDto()
        {
            InspPreguntasIncognitosRespuestas = new List<InspPreguntasIncognitosRespuestasDto>();
        }

        public string Descripcion { get; set; }
        public bool RespuestaRequerida { get; set; }
        public bool MostrarObservacion { get; set; }
        public int? Orden { get; set; }
        public bool Anulado { get; set; }

        public List<InspPreguntasIncognitosRespuestasDto> InspPreguntasIncognitosRespuestas { get; set; }
        public override string Description => this.Descripcion;
    }
}
