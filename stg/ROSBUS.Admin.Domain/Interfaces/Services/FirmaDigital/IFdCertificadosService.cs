using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IFdCertificadosService : IServiceBase<FdCertificados, int>
    {
        Task<List<FdCertificados>> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter);
        Task<FdCertificados> RevocarCertificado(FdCertificadosFilter filter);
        Task<string> GetEmployeeCuil(int currentuserID);
    }
}
