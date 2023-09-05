using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHMediasVueltasAppService : IAppServiceBase<HMediasVueltas, HMediasVueltasDto, int>
    {
        Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro);
        Task<ReportModel> GetDatosReportePuntaPunta(HMediasVueltasFilter filtro);
    }
}
