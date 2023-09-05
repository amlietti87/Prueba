using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IFdCertificadosAppService : IAppServiceBase<FdCertificados, FdCertificadosDto, int>
    {
        Task<List<FdCertificadosDto>> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter);
        Task<FdCertificadosDto> RevocarCertificado(FdCertificadosFilter filter);
        Task<FileDto> downloadCertificate(FdCertificadosFilter filter);
        Task<string> sendCertificateByEmail(FdCertificadosFilter filter);
    }
}
