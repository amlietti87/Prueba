using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class FdCertificadosService : ServiceBase<FdCertificados, int, IFdCertificadosRepository>, IFdCertificadosService
    { 
        public FdCertificadosService(IFdCertificadosRepository fdCertificadosRepository)
            : base(fdCertificadosRepository)
        {
       
        }

        public async Task<string> GetEmployeeCuil(int currentuserID)
        {
            return await this.repository.GetEmployeeCuil(currentuserID);
        }

        public async Task<List<FdCertificados>> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter)
        {
            return await this.repository.HistorialCertificadosPorUsuario(fdCertificadosFilter);
        }

        public async Task<FdCertificados> RevocarCertificado(FdCertificadosFilter filter)
        {
            return await this.repository.RevocarCertificado(filter);
        }
    }
    
}
