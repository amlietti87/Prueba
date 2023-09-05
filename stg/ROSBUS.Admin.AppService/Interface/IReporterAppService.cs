using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IReporterHttpAppService : IAppServiceBase<Reporter, ReporterDto, int>
    {
        Task<byte[]> GenerarReporteGenerico(ReportModel reporModel);
        Task<string> GenerarBase64DesdeSvg(string Svg);
    }
}
