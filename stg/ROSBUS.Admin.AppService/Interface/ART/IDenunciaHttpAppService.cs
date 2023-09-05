using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface.ART
{
    public interface IDenunciaHttpAppService : IAppServiceBase<ArtDenuncias, ArtDenunciasDto, int>
    {
        Task<byte[]> GetReport(DenunciaReportModel reportModel);
        Task<byte[]> GenerarReporteGenerico(ReportModel reportModel);
    }
}
