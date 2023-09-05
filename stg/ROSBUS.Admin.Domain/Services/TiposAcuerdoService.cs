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
    public class TiposAcuerdoService : ServiceBase<TiposAcuerdo,int, ITiposAcuerdoRepository>, ITiposAcuerdoService
    { 
        public TiposAcuerdoService(ITiposAcuerdoRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
