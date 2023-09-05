using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Operaciones.AppService.Interface
{
    public interface IEmpleadosAppService : IAppServiceBase<Empleados, EmpleadosDto, int>
    {
        Task<bool> ExisteEmpleado(int id);
        Task<bool> ExisteLegajoEmpleado(int id);
        Task<usuario> ObtenerEmpleadoPorDNI(string NroDoc);

    }
}
