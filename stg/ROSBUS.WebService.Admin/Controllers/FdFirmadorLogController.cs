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
    public class FdFirmadorLogController : ManagerController<FdFirmadorLog, long, FdFirmadorLogDto, FdFirmadorLogFilter, IFdFirmadorLogAppService>
    {
        public FdFirmadorLogController(IFdFirmadorLogAppService service)
            : base(service)
        {

        }


      

    }


 

}
