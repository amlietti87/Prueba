using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.WebService.Admin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class InsDesviosController : ManagerSecurityController<InsDesvios, long, InsDesviosDto, InsDesviosFilter, IInsDesviosAppService>
    {
        public InsDesviosController(IInsDesviosAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Inspectores", "Desvio");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleadoInspector()
        {
            try
            {
                var data = await Service.ObtenerEmpleadoInspector();
                return ReturnData<usuario>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<usuario>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDesviosPorUnidaddeNegocio()
        {
            try
            {
                var data =  await Service.GetDesviosPorUnidaddeNegocio();
                return ReturnData<List<InsDesviosDto>>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<InsDesviosDto>(ex);
            }

        }

    } 

}
