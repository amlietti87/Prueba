using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;

using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IFdFirmadorLogAppService : IAppServiceBase<FdFirmadorLog, FdFirmadorLogDto, long>
    {
    }
}
