using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class LesionadosService : ServiceBase<SinLesionados, int, ILesionadosRepository>, ILesionadosService
    { 
        public LesionadosService(ILesionadosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }




    }
    
}
