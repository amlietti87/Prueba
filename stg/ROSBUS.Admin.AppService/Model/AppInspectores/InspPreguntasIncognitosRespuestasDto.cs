using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspPreguntasIncognitosRespuestasDto : EntityDto<int>
    {

        public int PreguntaIncognitoId { get; set; }
        public int RespuestaId { get; set; }
        public int? Orden { get; set; }
        public string RespuestaNombre { get; set; }

        public override string Description => this.ToString();
    }
}
