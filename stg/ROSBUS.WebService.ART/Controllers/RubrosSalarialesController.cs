using Microsoft.AspNetCore.Mvc;
using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService.Model;
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
    public class RubrosSalarialesController : ManagerSecurityController<RubrosSalariales, int, RubrosSalarialesDto, RubrosSalarialesFilter, IRubrosSalarialesAppService>
    {
        public RubrosSalarialesController(IRubrosSalarialesAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Reclamo", "RubroSalarial");
        }

        public override Task<IActionResult> GetPagedList([FromBody] RubrosSalarialesFilter filter)
        {
            return base.GetPagedList(filter);
        }


    }


 

}
