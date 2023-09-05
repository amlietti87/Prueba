using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class LegajosEmpleadoController : ManagerController<LegajosEmpleado, int, LegajosEmpleadoDto, LegajosEmpleadoFilter, ILegajosEmpleadoAppService>
    {
        public LegajosEmpleadoController(ILegajosEmpleadoAppService service)
            : base(service)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetMaxById(int id)
        {
            try { 
            var data = await Service.GetMaxById(id);
            var dto = this.MapObject<LegajosEmpleado, LegajosEmpleadoDto>(data);
                return ReturnData<LegajosEmpleadoDto>(dto);
            }
            catch (Exception ex)
            {
                return ReturnError<LegajosEmpleadoDto> (ex);
            }
        }
    }


 

}
