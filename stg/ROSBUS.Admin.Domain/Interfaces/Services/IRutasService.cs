using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IRutasService : IServiceBase<GpsRecorridos, int>
    {
        Task<List<String>> ValidateAprobarRuta(GpsRecorridos entity);
        Task<List<HBasec>> RecuperarHbasecPorLinea(int cod_lin);
        Task<GpsRecorridos> TraerMinutosPorSector(MinutosPorSectorFilter filter);
        Task<GpsRecorridos> UpdateInstructions(GpsRecorridos entity);
    }
}
