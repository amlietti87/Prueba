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
    public class ReclamosHistoricosService : ServiceBase<SinReclamosHistoricos, int, IReclamosHistoricosRepository>, IReclamosHistoricosService
    { 
        public ReclamosHistoricosService(IReclamosHistoricosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }



    }
    
}
