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
    public class EmpleadosController : ManagerController<Empleados, int, EmpleadosDto, EmpleadosFilter, IEmpleadosAppService>
    {
        public EmpleadosController(IEmpleadosAppService service)
            : base(service)
        {

        }

        [HttpGet]
        public async Task<IActionResult> ExisteEmpleado(int id)
        {
            try
            {

                var data = await Service.ExisteEmpleado(id);
                return ReturnData<bool>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<bool>(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExisteLegajoEmpleado(int id)
        {
            try
            {

                var data = await Service.ExisteLegajoEmpleado(id);
                return ReturnData<bool>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<bool>(ex);
            }
        }
    }


 

}
