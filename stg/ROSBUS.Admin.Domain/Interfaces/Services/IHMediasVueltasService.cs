using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHMediasVueltasService : IServiceBase<HMediasVueltas, int>
    {
        Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro);
        Task<Empresa> GetCodigoEmpresa(decimal CodLinea);
    }
}
