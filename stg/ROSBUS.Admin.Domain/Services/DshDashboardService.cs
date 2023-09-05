using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class DshDashboardService : ServiceBase<DshDashboard,int, IDshDashboardRepository>, IDshDashboardService
    { 
        public DshDashboardService(IDshDashboardRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<List<DshUsuarioDashboardItem>> RecuperarDshUsuarioDashboardItems(int userId)
        {
            return this.repository.RecuperarDshUsuarioDashboardItems(userId);
        }

        public async Task SaveDashboardUsuario(IEnumerable<DshUsuarioDashboardItem> items, int DashboardLayoutId)
        {
            await this.repository.SaveDashboardUsuario(items, DashboardLayoutId);
        }
        
    }
    
}
