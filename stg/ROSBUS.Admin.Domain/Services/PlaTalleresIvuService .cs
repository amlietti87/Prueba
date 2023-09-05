using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PlaTalleresIvuService : ServiceBase<PlaTalleresIvu, int, IPlaTalleresIvuRepository>, IPlaTalleresIvuService
    { 
        public PlaTalleresIvuService(IPlaTalleresIvuRepository produtoRepository)
            : base(produtoRepository)
        {
        }

        public async Task<Galpon> GetGalponWithIvu(int CodUbicacion)
        {
            return await this.repository.GetGalponWithIvu(CodUbicacion);
        }
    }
    
}
