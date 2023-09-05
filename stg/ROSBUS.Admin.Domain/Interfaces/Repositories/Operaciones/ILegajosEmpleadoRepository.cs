using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Operaciones.Domain.Interfaces.Repositories
{
    public interface ILegajosEmpleadoRepository : IRepositoryBase<LegajosEmpleado, int>
    {

        Task<LegajosEmpleado> GetMaxById(int id);
    }
}
