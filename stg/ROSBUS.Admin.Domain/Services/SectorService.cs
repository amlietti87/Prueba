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
    public class SectorService : ServiceBase<PlaSector,Int64, ISectorRepository>, ISectorService
    { 
        public SectorService(ISectorRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<PlaSentidoPorSector>> RecuperarSentidoPorSector(HDesignarFilter Filtro)
        {
            var sectores = await repository.RecuperarSentidoPorSector(Filtro);

            return sectores;
        }

    }
    
}
