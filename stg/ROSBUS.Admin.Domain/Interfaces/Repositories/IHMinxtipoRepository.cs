using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHMinxtipoRepository: IRepositoryBase<HMinxtipo,int>
    {
        Task<List<HSectores>> GetHSectores(HMinxtipoFilter filter);
        Task UpdateHMinxtipo(IEnumerable<HMinxtipo> items);
        Task ImportarMinutos(ImportadorHMinxtipoResult planilla);
        Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input);
        Task SetHSectores(IEnumerable<HSectores> entities);
    }
}
