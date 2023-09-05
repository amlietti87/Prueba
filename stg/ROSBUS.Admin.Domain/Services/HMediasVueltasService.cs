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
    public class HMediasVueltasService : ServiceBase<HMediasVueltas,int, IHMediasVueltasRepository>, IHMediasVueltasService
    { 
        public HMediasVueltasService(IHMediasVueltasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro)
        {
            return await this.repository.LeerMediasVueltasIncompletas(Filtro);
        }


        public async Task<Empresa> GetCodigoEmpresa(decimal CodLinea)
        {
            return await this.repository.GetCodigoEmpresa(CodLinea);
        }
    }
    
}
