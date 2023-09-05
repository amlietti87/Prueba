using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PlaEstadoHorarioFechaService : ServiceBase<PlaEstadoHorarioFecha,int, IPlaEstadoHorarioFechaRepository>, IPlaEstadoHorarioFechaService
    { 
        public PlaEstadoHorarioFechaService(IPlaEstadoHorarioFechaRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
