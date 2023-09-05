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
    public class InspDiagramasInspectoresService : ServiceBase<InspDiagramasInspectores, int, IInspDiagramasInspectoresRepository>, IInspDiagramasInspectoresService
    {
        public InspDiagramasInspectoresService(IInspDiagramasInspectoresRepository Repository)
            : base(Repository)
        {
        
        }

        public  Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity)
        {
            return this.repository.DiagramaMesAnioGrupo(Id, turnoId, blockentity);
        }

        public Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha)
        {
            return this.repository.DiagramacionPorDia(Fecha);
        }
        public Task<InspectorDiaDto>EliminarCelda(DiasMesDto model)
        {
            
            return this.repository.EliminarCelda(model);
        }

        public Task SaveDiagramacion(List<HFrancos> hFrancos, List<PersJornadasTrabajadas> jornadasTrabajadas)
        {
            return this.repository.SaveDiagramacion(hFrancos, jornadasTrabajadas);
        }
    }
}
