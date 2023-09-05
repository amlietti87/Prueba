using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface ICoordenadasService : IServiceBase<PlaCoordenadas, int>
    {
       Task<List<PlaCoordenadas>> RecuperarCoordenadasPorFecha(CoordenadasFilter filter);
    }
}
