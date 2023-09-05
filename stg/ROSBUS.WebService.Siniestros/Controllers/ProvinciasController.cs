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
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class ProvinciasController : ManagerSecurityController<Provincias, int, ProvinciasDto, ProvinciasFilter, IProvinciasAppService>
    {
        public ProvinciasController(IProvinciasAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "Localidad");
        }

        public override Task<IActionResult> GetAllAsync(ProvinciasFilter filter)
        {
            return base.GetAllAsync(filter);
        }

        public override Task<IActionResult> GetItemsAsync(ProvinciasFilter filter)
        {
            return base.GetItemsAsync(filter);
        }

        public override Task<IActionResult> GetPagedList([FromBody] ProvinciasFilter filter)
        {
            return base.GetPagedList(filter);
        }
    }


 

}
