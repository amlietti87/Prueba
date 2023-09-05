using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class FdFirmadorService : ServiceBase<FdFirmador,long, IFdFirmadorRepository>, IFdFirmadorService
    { 
        public FdFirmadorService(IFdFirmadorRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<FdFirmador> GetFirmadorByToken(string token, int idRecibo)
        {
            return this.repository.GetFirmadorByToken(token, idRecibo);
        }

        public Task UpdateLogs(FdFirmador entity)
        {
            return this.repository.UpdateLogs(entity);
        }
    }
    
}
