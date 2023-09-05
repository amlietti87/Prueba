using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class PlaTipoViajeController : ManagerSecurityController<PlaTipoViaje, int, PlaTipoViajeDto, PlaTipoViajeFilter, IPlaTipoViajeAppService>
    {
        public PlaTipoViajeController(IPlaTipoViajeAppService service) : base(service)
        {

        }

        protected override void InitializePermission()
        {

        }
    }
}