using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHChoxserRepository : IRepositoryBase<HChoxser, int>
    {
        Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter);
        Task ImportarDuraciones(ImportadorHChoxserResult input);
        Task DeleteDuracionesServicio(int idServicio);
    }
}
