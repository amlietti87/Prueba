﻿using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class SysDataTypesAppService : AppServiceBase<SysDataTypes, SysDataTypesDto, int, ISysDataTypesService>, ISysDataTypesAppService
    {
        public SysDataTypesAppService(ISysDataTypesService serviceBase)
            :base(serviceBase)
        {

        }
    }
}
