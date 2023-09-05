using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IPlaDistribucionDeCochesPorTipoDeDiaRepository : IRepositoryBase<PlaDistribucionDeCochesPorTipoDeDia, int>
    {
        Task ImportarServiciosAsync(ImportarServiciosInput input);
        Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input);
        Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input);
        Task<Boolean> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter);
        Task<PlaDuracionesEstadoView> ExistenDuracionesIncompletas(PlaDistribucionDeCochesPorTipoDeDia item);
    }
}
