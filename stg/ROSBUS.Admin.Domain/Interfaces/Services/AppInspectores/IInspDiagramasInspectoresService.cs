using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service
{
    public interface IInspDiagramasInspectoresService : IServiceBase<InspDiagramasInspectores, int>
    {
        Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity);
        Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha);
        Task<InspectorDiaDto> EliminarCelda(DiasMesDto model);
        Task SaveDiagramacion(List<HFrancos> hFrancos, List<PersJornadasTrabajadas> jornadasTrabajadas);
    }
}