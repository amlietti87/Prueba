using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class GalponService : ServiceBase<Galpon, Decimal, IGalponRepository>, IGalponService
    { 
        public GalponService(IGalponRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task UpdateList(IEnumerable<Galpon> talleres)
        {
            await this.repository.UpdateList(talleres);
        }
    }
    
}
