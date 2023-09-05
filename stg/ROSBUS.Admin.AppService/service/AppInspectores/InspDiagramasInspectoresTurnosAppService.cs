using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class InspDiagramasInspectoresTurnosAppService : AppServiceBase <InspDiagramasInspectoresTurnos, InspDiagramasInspectoresTurnosDto, int, IInspDiagramasInspectoresTurnosService>, IInspDiagramasInspectoresTurnosAppService
    {
        public InspDiagramasInspectoresTurnosAppService(IInspDiagramasInspectoresTurnosService serviceBase)
             : base(serviceBase)
        {

        }       
    }
}
