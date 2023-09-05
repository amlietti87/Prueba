using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IRutasRepository : IRepositoryBase<GpsRecorridos, int>
    {

        Task<List<HBasec>> RecuperarHbasecPorLinea(int cod_lin);
        Task<GpsRecorridos> TraerMinutosPorSector(MinutosPorSectorFilter filter);
        Task<int?> GetEstadoOriginal(int id);
        Task<GpsRecorridos> UpdateInstructions(GpsRecorridos entity);
    }
}
