using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class sucursalesxLineasService : ServiceBase<SucursalesxLineas,int, IsucursalesxLineasRepository>, IsucursalesxLineasService
    { 
        public sucursalesxLineasService(IsucursalesxLineasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
