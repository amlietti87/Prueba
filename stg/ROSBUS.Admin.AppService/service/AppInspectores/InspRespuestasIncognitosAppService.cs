using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class InspRespuestasIncognitosAppService : AppServiceBase <InspRespuestasIncognitos, InspRespuestasIncognitosDto, int, IInspRespuestasIncognitosService>, IInspRespuestasIncognitosAppService
    {
        public InspRespuestasIncognitosAppService(IInspRespuestasIncognitosService serviceBase)
             : base(serviceBase)
        {

        }
    }
}
