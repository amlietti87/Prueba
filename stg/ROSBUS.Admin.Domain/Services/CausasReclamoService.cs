using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Interfaces.Repositories;
using ROSBUS.ART.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.ART.Domain.Services
{
    public class CausasReclamoService : ServiceBase<CausasReclamo,int, ICausasReclamoRepository>, ICausasReclamoService
    { 
        public CausasReclamoService(ICausasReclamoRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
