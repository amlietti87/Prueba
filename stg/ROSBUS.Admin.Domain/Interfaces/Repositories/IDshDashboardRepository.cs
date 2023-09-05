using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IDshDashboardRepository : IRepositoryBase<DshDashboard, int>
    {
        Task SaveDashboardUsuario(IEnumerable<DshUsuarioDashboardItem> items, int DashboardLayoutId);
        Task<List<DshUsuarioDashboardItem>> RecuperarDshUsuarioDashboardItems(int userId);
    }
}
