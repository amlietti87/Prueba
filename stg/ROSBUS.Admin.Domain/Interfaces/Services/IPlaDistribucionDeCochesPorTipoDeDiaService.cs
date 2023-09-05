using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IPlaDistribucionDeCochesPorTipoDeDiaService : IServiceBase<PlaDistribucionDeCochesPorTipoDeDia, int>
    {
        Task<List<HMediasVueltasImportadaDto>> RecuperarPlanilla(PlaDistribucionDeCochesPorTipoDeDiaFilter filter);
        Task ImportarServicios(ImportarServiciosInput input);
        Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input);
        Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input);
        Task<Boolean> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter);
        Task<PlaDuracionesEstadoView> ExistenDuracionesIncompletas(PlaDistribucionDeCochesPorTipoDeDia item);
    }
}
