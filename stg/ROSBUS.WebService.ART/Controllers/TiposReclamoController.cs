using Microsoft.AspNetCore.Mvc;
using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.ART.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class TiposReclamoController : ManagerSecurityController<TiposReclamo, int, TiposReclamoDto, TiposReclamoFilter, ITiposReclamoAppService>
    {
        public TiposReclamoController(ITiposReclamoAppService service)
            : base(service)
        {

        }



        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Reclamo", "TipoReclamo");
        }

        public override Task<IActionResult> GetPagedList([FromBody] TiposReclamoFilter filter)
        {
            return base.GetPagedList(filter);
        }

        public override Task<IActionResult> SaveNewEntity([FromBody] TiposReclamoDto dto)
        {
            return base.SaveNewEntity(dto);
        }
        public override Task<IActionResult> UpdateEntity([FromBody] TiposReclamoDto dto)
        {
            return base.UpdateEntity(dto);
        }
    }




}
