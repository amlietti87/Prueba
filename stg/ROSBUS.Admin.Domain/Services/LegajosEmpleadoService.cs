using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters; 
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Operaciones.Domain.Services
{
    public class LegajosEmpleadoService : ServiceBase<LegajosEmpleado,int, ILegajosEmpleadoRepository>, ILegajosEmpleadoService
    { 
        public LegajosEmpleadoService(ILegajosEmpleadoRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<LegajosEmpleado> GetMaxById(int id)
        {
            return await this.repository.GetMaxById(id);
        }
    }
    
}
