using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspPreguntasIncognitosRespuestasService : ServiceBase<InspPreguntasIncognitosRespuestas, int, IInspPreguntasIncognitosRespuestasRepository>, IInspPreguntasIncognitosRespuestasService
    {
        public InspPreguntasIncognitosRespuestasService(IInspPreguntasIncognitosRespuestasRepository respuestasRepository)
            : base(respuestasRepository)
        {

        }
    }
}
