using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IAbogadosAppService : IAppServiceBase<SinAbogados, AbogadosDto, int>
    {
    }
}
