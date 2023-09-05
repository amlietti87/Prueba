using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class MuebleInmuebleService : ServiceBase<SinMuebleInmueble, int, IMuebleInmuebleRepository>, IMuebleInmuebleService
    { 
        public MuebleInmuebleService(IMuebleInmuebleRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }




    }
    
}
