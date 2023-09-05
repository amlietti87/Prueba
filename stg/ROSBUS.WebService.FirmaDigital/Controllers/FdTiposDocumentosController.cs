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
    public class FdTiposDocumentosController : ManagerSecurityController<FdTiposDocumentos, int, FdTiposDocumentosDto, FdTiposDocumentosFilter, IFdTiposDocumentosAppService>
    {
        private readonly IFdAccionesAppService _accionesService;

        public FdTiposDocumentosController(IFdTiposDocumentosAppService service, IFdAccionesAppService accionesService)
            : base(service)
        {
            _accionesService = accionesService;
        }

        [HttpPost]
        public override Task<IActionResult> DeleteById(int Id)
        {
            //var accionesRelatedToTipoDocumento = this._accionesService.GetAllAsync(e => e.TipoDocumentoId == Id).Result.Items;
            //foreach(var accion in accionesRelatedToTipoDocumento)
            //{
            //    this._accionesService.Delete(accion.Id);
            //}
            return base.DeleteById(Id);
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("FirmaDigital", "TipoDocumento");
        }

    }


 

}
