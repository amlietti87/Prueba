using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Interfaces.Repositories;
using ROSBUS.ART.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.ART.Domain.Services
{
    public class TiposReclamoService : ServiceBase<TiposReclamo,int, ITiposReclamoRepository>, ITiposReclamoService
    { 
        public TiposReclamoService(ITiposReclamoRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
