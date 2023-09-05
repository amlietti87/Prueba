using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.ART.AppService.Interface
{
    public interface ICausasReclamoAppService : IAppServiceBase<CausasReclamo, CausasReclamoDto, int>
    {
    }
}
