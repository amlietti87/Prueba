using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class EmpresaController : ManagerSecurityController<Empresa, Decimal, EmpresaDto, EmpresaFilter, IEmpresaAppService>
    {
        public EmpresaController(IEmpresaAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "Empresa");
        }

        public async override Task<IActionResult> GetAllAsync(EmpresaFilter filter)
        {
            return await base.GetAllAsync(filter);
        }
    }
}
