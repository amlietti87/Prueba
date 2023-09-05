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
    public class SubGalponService : ServiceBase<SubGalpon,Decimal, ISubGalponRepository>, ISubGalponService
    { 
        public SubGalponService(ISubGalponRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<SubGalpon> GetSubGalponWithLineaAndGalpon (decimal CodSubGalpon, decimal CodLin)
        {
            return await this.repository.GetSubGalponWithLineaAndGalpon(CodSubGalpon, CodLin);
        }

        public async Task<List<SubGalpon>> GetAllWithConfigu()
        {
            return await this.repository.GetAllWithConfigu();
        }

    }
    
}
