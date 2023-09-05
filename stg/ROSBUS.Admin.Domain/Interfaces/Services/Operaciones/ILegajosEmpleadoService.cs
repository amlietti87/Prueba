using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Operaciones.Domain.Entities; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface ILegajosEmpleadoService : IServiceBase<LegajosEmpleado, int>
    {

        Task<LegajosEmpleado> GetMaxById(int id);
    }
}
