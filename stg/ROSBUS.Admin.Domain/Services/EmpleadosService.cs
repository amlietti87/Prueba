using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
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
    public class EmpleadosService : ServiceBase<Empleados,int, IEmpleadosRepository>, IEmpleadosService
    { 
        public EmpleadosService(IEmpleadosRepository produtoRepository)
            : base(produtoRepository)
        {
        
        }

        public async Task<bool> ExisteEmpleado(int id)
        {
            return await this.repository.ExisteEmpleado(id);
        }
        public async Task<bool> ExisteLegajoEmpleado(int id)
        {
            return await this.repository.ExisteLegajoEmpleado(id);
        }    

        public async Task<usuario> ObtenerEmpleadoPorDNI(string NroDoc)
        {
            return await this.repository.ObtenerEmpleadoPorDNI(NroDoc);
        }

    }
    
}
