using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHMinxtipoService : IServiceBase<HMinxtipo, int>
    {
        Task<List<HSectores>> GetHSectores(HMinxtipoFilter filter);
        Task UpdateHMinxtipo(IEnumerable<HMinxtipo> items);
        Task<ImportadorHMinxtipoResult> RecuperarPlanilla(HMinxtipoFilter filter);
        Task ImportarMinutos(HMinxtipoImporarInput input);
        Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input);
        Task SetHSectores(IEnumerable<HSectores> entities);

    }
}
