using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class DshDashboardAppService : AppServiceBase<DshDashboard, DshDashboardDto, int, IDshDashboardService>, IDshDashboardAppService
    {
        private IAuthService _authService;
        private readonly IUserService userService;

        public DshDashboardAppService(IDshDashboardService serviceBase, IAuthService authService, IUserService _userService) 
            :base(serviceBase)
        {
            this._authService = authService;
            this.userService = _userService;
        }

        public async Task GuardarDashboard(UsuarioDashboardInput dto)
        {
            IEnumerable<DshUsuarioDashboardItem> items = this.MapList<UsuarioDashboardItemDto, DshUsuarioDashboardItem>(dto.Items);

            await this._serviceBase.SaveDashboardUsuario(items, dto.DashboardLayoutId);
        }

        public async Task<List<UsuarioDashboardItemDto>> RecuperarDshUsuarioDashboardItems(int userId)
        {
            List<DshUsuarioDashboardItem> items = await this._serviceBase.RecuperarDshUsuarioDashboardItems(userId);

            return this.MapList<DshUsuarioDashboardItem, UsuarioDashboardItemDto>(items).ToList();

        }
    }
}
