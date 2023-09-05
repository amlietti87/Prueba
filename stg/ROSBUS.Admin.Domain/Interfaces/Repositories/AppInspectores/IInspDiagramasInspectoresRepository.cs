using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces
{
    public interface IInspDiagramasInspectoresRepository : IRepositoryBase<InspDiagramasInspectores, int>
    {
        Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity);
        Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha);
        Task<InspectorDiaDto> EliminarCelda(DiasMesDto model);

        Task SaveDiagramacion(List<HFrancos> hFrancos, List<PersJornadasTrabajadas> jornadasTrabajadas);

    }
}
