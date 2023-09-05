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
    public class InspGrupoInspectoresRangosHorariosController : ManagerController<InspGrupoInspectoresRangosHorarios, int, InspGrupoInspectoresRangosHorariosDto, InspGrupoInspectoresRangosHorariosFilter, IInspGruposInspectoresRangosHorariosAppService>
    {
        public InspGrupoInspectoresRangosHorariosController(IInspGruposInspectoresRangosHorariosAppService service)
            : base(service)
        {

        }


    }


 

}
