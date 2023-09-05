
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class TipoLineaController : ManagerSecurityController<PlaTipoLinea, int, TipoLineaDto, TipoLineaFilter, ITipoLineaAppService>
    {
        public TipoLineaController(ITipoLineaAppService service)
            : base(service)
        {

        }
        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "TipoDeLinea");
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarTipoLineaPorSector(HDesignarFilter Filtro)
        {
            try
            {
                var tlineas = await Service.RecuperarTipoLineaPorSector(Filtro);
                return ReturnData<List<PlaTipoLinea>>(tlineas);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<PlaTipoLinea>>(ex);
            }
        }
    }

   
    




}
