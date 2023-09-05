using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class VehiculosService : ServiceBase<SinVehiculos, int, IVehiculosRepository>, IVehiculosService
    { 
        public VehiculosService(IVehiculosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }




    }
    
}
