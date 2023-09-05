using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IPuntosRepository: IRepositoryBase<PlaPuntos, Guid>
    {
        Task<List<GpsDetaReco>> RecuperarDatosIniciales(int CodRec);

        Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf);
        Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec);
    }
}
