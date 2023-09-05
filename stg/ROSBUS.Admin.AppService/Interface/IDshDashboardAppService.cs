using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IDshDashboardAppService : IAppServiceBase<DshDashboard, DshDashboardDto, int>
    {
        Task GuardarDashboard(UsuarioDashboardInput dto);
        Task<List<UsuarioDashboardItemDto>> RecuperarDshUsuarioDashboardItems(int userId);
    }
}
