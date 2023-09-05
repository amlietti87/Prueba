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
    public class PlaParadasService : ServiceBase<PlaParadas,int, IPlaParadasRepository>, IPlaParadasService
    { 
        public PlaParadasService(IPlaParadasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<PlaParadas>> ParadasBuscarCercanos(PlaParadasFilter filter)
        {
            return await this.repository.ParadasBuscarCercanos(filter);
        }
    }
    
}
