using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PuntosService : ServiceBase<PlaPuntos, Guid, IPuntosRepository>, IPuntosService
    { 
        public PuntosService(IPuntosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf)
        {
            return await this.repository.GetFilterPuntosInicioFin(pf);
        }

        public async Task<List<GpsDetaReco>> RecuperarDatosIniciales(int CodRec)
        {
            return await this.repository.RecuperarDatosIniciales(CodRec);
        }

        public async Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec)
        {
            return await this.repository.RecuperarDetaReco(CodRec);
        }
    }
    
}
