using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.Admin.AppService.Interface.ART
{
    public interface IMotivosNotificacionesAppService : IAppServiceBase<ArtMotivosNotificaciones, ArtMotivosNotificacionesDto, int>
    {
    }
}
