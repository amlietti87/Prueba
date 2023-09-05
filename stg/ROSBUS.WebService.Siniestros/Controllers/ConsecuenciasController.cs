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
    public class ConsecuenciasController : ManagerSecurityController<SinConsecuencias, int, ConsecuenciasDto, ConsecuenciasFilter, IConsecuenciasAppService>
    {
        public ConsecuenciasController(IConsecuenciasAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "Consecuencia");
        }

        [HttpGet]
        public async Task<IActionResult> GetConsecuenciasSinAnular()
        {
            try
            {

                var data = await Service.GetConsecuenciasSinAnular();
                var dto = this.MapObject<List<SinConsecuencias>, List<ConsecuenciasDto>>(data);
                return ReturnData<List<ConsecuenciasDto>>(dto);
            }
            catch (Exception ex)
            {
                return ReturnError<List<ConsecuenciasDto>>(ex);
            }
        }
    }


 

}
