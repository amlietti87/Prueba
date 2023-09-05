using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface ISiniestrosHttpAppService : IAppServiceBase<SinSiniestros, SiniestrosDto, int>
    {
        Task<byte[]> GetReport(SiniestroReportModel reportModel);


        Task<byte[]> GenerarReporteGenerico(ReportModel reportModel);
        Task<string> GenerarBase64DesdeSvg(string Svg);
    }
}
