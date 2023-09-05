using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class InspPlanillaIncognitosDetalleDto :EntityDto<int>
    {
        public int PreguntaIncognitoId { get; set; }
        public int? RespuestaIncognitoId { get; set; }
        public  string Observacion { get; set; }
        public override string Description => "";
    }
}
