using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;

using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System.Threading.Tasks;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IFdFirmadorAppService : IAppServiceBase<FdFirmador, FdFirmadorDto, long>
    {
        Task UpdateLogs(FdFirmadorDto dto);
        Task<FdFirmadorDto> GetFirmadorByToken(string token, int idRecibo);
    }
}
