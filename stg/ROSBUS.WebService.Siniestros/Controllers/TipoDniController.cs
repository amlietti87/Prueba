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
    public class TipoDniController : ManagerSecurityController<TipoDni, int, TipoDniDto, TipoDniFilter, ITipoDniAppService>
    {
        public TipoDniController(ITipoDniAppService service)
            : base(service)
        {

        }

        public override Task<IActionResult> GetAllAsync(TipoDniFilter filter)
        {
            return base.GetAllAsync(filter);
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "TipoDni");
        }
    }


 

}
