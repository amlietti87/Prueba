using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class FactoresIntervinientesController : ManagerSecurityController<SinFactoresIntervinientes, int, FactoresIntervinientesDto, FactoresIntervinientesFilter, IFactoresIntervinientesAppService>
    {
        public FactoresIntervinientesController(IFactoresIntervinientesAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "FactoresIntervinientes");
        }
    }


 

}
