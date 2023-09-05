using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;
using TECSO.FWK.Extensions;
using Microsoft.Extensions.Configuration;

namespace ROSBUS.Admin.AppService.service.ART
{
    public class DenunciaHttpAppService : HttpAppServiceBase<ArtDenuncias, ArtDenunciasDto, int>, IDenunciaHttpAppService
    {
        public override string EndPoint => "api/Report";
        public string urlPathReport { get; }
        public DenunciaHttpAppService(IAuthService authService)
            : base(authService)
        {
            urlPathReport = string.Format("{0}{1}/", GetUrlReporting().EnsureEndsWith('/'), this.EndPoint);
            BuildClientHttpReport();
        }
        protected void BuildClientHttpReport()
        {
            this.httpClient = new HttpCustomClient(urlPathReport, ()=> authService.GetCurretToken());
        }


        protected string GetUrlReporting()
        {
            return configuration.GetValue<string>("ReportUrl");
        }

        public async Task<byte[]> GetReport(DenunciaReportModel reportModel)
        {

            string action = "GenerarReporte";
            var bytes = await this.httpClient.PostRequestResponseModel<byte[], DenunciaReportModel>(action, reportModel);

            return bytes;
        }

        public async Task<byte[]> GenerarReporteGenerico(ReportModel reporModel)
        {
            string action = "GenerarReporteGenerico";
            var bytes = await this.httpClient.PostRequestResponseModel<byte[], ReportModel>(action, reporModel);

            return bytes;
        }
    }
}
