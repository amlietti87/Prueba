using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService
{
    public class InfResponsablesAppService : AppServiceBase<InfResponsables, InfResponsablesDto, string, IInfResponsablesService>, IInfResponsablesAppService
    {
        public InfResponsablesAppService(IInfResponsablesService serviceBase)
            : base(serviceBase)
        {

        }
    }
}
