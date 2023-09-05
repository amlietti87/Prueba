using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IPuntosService : IServiceBase<PlaPuntos, Guid>
    {
        Task<List<GpsDetaReco>> RecuperarDatosIniciales(int CodRec);

        Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec);
        Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf);




        
    }
}
