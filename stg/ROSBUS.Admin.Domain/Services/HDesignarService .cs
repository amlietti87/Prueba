using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HDesignarService : ServiceBase<HDesignar, int, IHDesignarRepository>, IHDesignarService
    { 
        public HDesignarService(IHDesignarRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }
        public async Task<List<HDesignarSabanaSector>> RecuperarSabanaPorSector(HDesignarFilter Filter)
        {
            var sectores = await repository.RecuperarSabanaPorSector(Filter);

            return sectores;
        }
    }
    
}
