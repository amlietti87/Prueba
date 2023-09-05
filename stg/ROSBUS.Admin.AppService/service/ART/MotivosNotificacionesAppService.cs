﻿using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.AppService;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.Admin.AppService.service.ART
{
    public class MotivosNotificacionesAppService : AppServiceBase<ArtMotivosNotificaciones, ArtMotivosNotificacionesDto, int, IMotivosNotificacionesService>, IMotivosNotificacionesAppService
    {
        public MotivosNotificacionesAppService(IMotivosNotificacionesService serviceBase) : base(serviceBase)
        {
        }

    }
}