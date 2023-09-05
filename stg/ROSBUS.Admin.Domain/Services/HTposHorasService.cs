using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HTposHorasService : ServiceBase<HTposHoras,string, IHTposHorasRepository>, IHTposHorasService
    { 
        public HTposHorasService(IHTposHorasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
