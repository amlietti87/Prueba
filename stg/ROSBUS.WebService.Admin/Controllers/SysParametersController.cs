using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.ParametersHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class SysParametersController : ManagerSecurityController<SysParameters, long, SysParametersDto, SysParametersFilter, ISysParametersAppService>
    {
        private readonly IParametersHelper parametersHelper;

        public SysParametersController(ISysParametersAppService service, IParametersHelper _parametersHelper)
            : base(service)
        {
            this.parametersHelper = _parametersHelper;
        }
        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "Parametros");
        }

        [HttpGet]
        public virtual IActionResult GetByToken(string token)
        {
            try
            {
                return ReturnData<string>(this.parametersHelper.GetParameter<string>(token));
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

    }
}
