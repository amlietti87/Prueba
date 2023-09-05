using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHMediasVueltasRepository: IRepositoryBase<HMediasVueltas,int>
    {
        Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro);
        Task<Empresa> GetCodigoEmpresa(decimal CodLinea);
    }
}
