using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspDiagramasInspectoresTurnosService : ServiceBase<InspDiagramasInspectoresTurnos, int, IInspDiagramasInspectoresTurnosRepository>, IInspDiagramasInspectoresTurnosService
    {
        public InspDiagramasInspectoresTurnosService(IInspDiagramasInspectoresTurnosRepository Repository)
            : base(Repository)
        {
        
        }
       
    }
}
