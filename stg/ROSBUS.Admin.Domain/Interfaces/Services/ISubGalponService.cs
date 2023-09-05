using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface ISubGalponService : IServiceBase<SubGalpon, Decimal>
    {
        Task<SubGalpon> GetSubGalponWithLineaAndGalpon(decimal CodSubGalpon, decimal CodLin);
        Task<List<SubGalpon>> GetAllWithConfigu();
    }
}
