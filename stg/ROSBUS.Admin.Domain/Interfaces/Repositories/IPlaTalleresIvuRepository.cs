using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IPlaTalleresIvuRepository: IRepositoryBase<PlaTalleresIvu, int>
    {
        Task<Galpon> GetGalponWithIvu(int CodUbicacion);
    }
}
