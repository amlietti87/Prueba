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

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class PlaEstadoHorarioFechaController : ManagerController<PlaEstadoHorarioFecha, int, PlaEstadoHorarioFechaDto, PlaEstadoHorarioFechaFilter, IPlaEstadoHorarioFechaAppService>
    {
        public PlaEstadoHorarioFechaController(IPlaEstadoHorarioFechaAppService service)
            : base(service)
        {

        }


      

    }


 

}
