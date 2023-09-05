using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface.AppInspectores
{
    public interface IInspDiagramasInspectoresAppService : IAppServiceBase<InspDiagramasInspectores, InspDiagramasInspectoresDto, int>
    {
        Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity);
        Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha);
        Task<List<InspDiagramasInspectoresTurnosDto>> TurnosDeLaDiagramacion(int Id);

        Task<InspectorDiaDto> EliminarCelda(DiasMesDto model);
        Task SaveDiagramacion(List<InspectorDiaDto> InspectoresDto, int Id);
        Task PublicarDiagramacion(InspDiagramasInspectores Diagramacion);
        Task<FileDto> ImprimirDiagrama(int Id, List<int> turnoId);
        
    }

}
