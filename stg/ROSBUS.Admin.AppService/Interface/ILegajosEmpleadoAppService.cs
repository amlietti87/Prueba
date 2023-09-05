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
    public interface ILegajosEmpleadoAppService : IAppServiceBase<LegajosEmpleado, LegajosEmpleadoDto, int>
    {
        Task<LegajosEmpleado> GetMaxById(int id);
    }
}
