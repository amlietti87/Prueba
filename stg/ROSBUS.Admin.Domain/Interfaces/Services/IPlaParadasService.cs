using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IPlaParadasService : IServiceBase<PlaParadas, int>
    {
        Task<List<PlaParadas>> ParadasBuscarCercanos(PlaParadasFilter filter);
    }
}
