using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService.service
{
    public class ReporterHttpAppService : HttpAppServiceBase<Reporter, ReporterDto, int>, IReporterHttpAppService
    {
        public override string EndPoint => "api/Report";

      

        public ReporterHttpAppService(IAuthService authService)
            : base(authService)
        {

        }

        protected override string GetUrlBase()
        {
            return configuration.GetValue<string>("ReportUrl").EnsureEndsWith('/');
        }
        public async Task<byte[]> GenerarReporteGenerico(ReportModel reporModel)
        {
            string action = "GenerarReporteGenerico";
            var bytes = await this.httpClient.PostRequestResponseModel<byte[], ReportModel>(action, reporModel);

            return bytes;
        }

        public async Task<string> GenerarBase64DesdeSvg(string Svg)
        {
            string action = "GenerarBase64DesdeSvg";
            var svgbase64 = await this.httpClient.PostRequestResponseModel<string, string>(action, Svg);

            return svgbase64;
        }

    }
}
