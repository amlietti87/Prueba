using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class ReclamoCuotasService : ServiceBase<SinReclamoCuotas, int, IReclamoCuotasRepository>, IReclamoCuotasService
    { 
        public ReclamoCuotasService(IReclamoCuotasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }




    }
    
}
