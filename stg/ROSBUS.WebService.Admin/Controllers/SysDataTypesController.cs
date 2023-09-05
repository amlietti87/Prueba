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
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class SysDataTypesController : ManagerController<SysDataTypes, int, SysDataTypesDto, SysDataTypesFilter, ISysDataTypesAppService>
    {
        public SysDataTypesController(ISysDataTypesAppService service)
            : base(service)
        {

        }
    }
}
