using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.ART.Domain.Entities.ART;

namespace ROSBUS.ART.AppService.Interface
{
    public interface ITiposReclamoAppService : IAppServiceBase<TiposReclamo, TiposReclamoDto, int>
    {
    }
}
