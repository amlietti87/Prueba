using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface ICroElemenetoAppService : IAppServiceBase<CroElemeneto, CroElemenetoDto, Guid>
    {
        Task<AdjuntosDto> GetAdjunto(Guid Id);
    }
}
