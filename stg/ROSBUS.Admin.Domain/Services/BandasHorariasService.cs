using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class BandasHorariasService : ServiceBase<PlaBandasHorarias,int, IBandasHorariasRepository>, IBandasHorariasService
    { 
        public BandasHorariasService(IBandasHorariasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
