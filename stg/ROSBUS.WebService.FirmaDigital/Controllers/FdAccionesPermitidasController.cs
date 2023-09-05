using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class FdAccionesPermitidasController : ManagerController<FdAccionesPermitidas, int, FdAccionesPermitidasDto, FdAccionesPermitidasFilter, IFdAccionesPermitidasAppService>
    {
        public FdAccionesPermitidasController(IFdAccionesPermitidasAppService service)
            : base(service)
        {

        }




    }


 

}
