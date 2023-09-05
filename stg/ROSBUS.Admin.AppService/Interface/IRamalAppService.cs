using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IRamalColorAppService : IAppServiceBase<PlaRamalColor, RamalColorDto, Int64>
    {
        bool TieneMapasEnBorrador(int id);
        Task<List<TECSO.FWK.Domain.Interfaces.Entities.ItemDto<long>>> GetItemsAsyncSinSentidos(RamalColorFilter filter);
    }
}
