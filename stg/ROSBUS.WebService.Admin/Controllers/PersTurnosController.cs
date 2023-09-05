using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class PersTurnosController : ManagerSecurityController<PersTurnos, int, PersTurnosDto, PersTurnosFilter, IPersTurnosAppService>
    {
        public PersTurnosController(IPersTurnosAppService service)
            : base(service)
        {

        }

        //protected override void InitializePermission()
        //{
        //    this.InitializePermissionByDefault("Inspectores", " PersTurnos");
        //}

    }


 

}
