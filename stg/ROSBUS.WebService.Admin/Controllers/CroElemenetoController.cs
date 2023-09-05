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
    public class CroElemenetoController : ManagerController<CroElemeneto, Guid, CroElemenetoDto, CroElemenetoFilter, ICroElemenetoAppService>
    {

        private readonly IAdjuntosAppService adjuntosAppService;
        public CroElemenetoController(ICroElemenetoAppService service, IAdjuntosAppService adjuntosAppService)
            : base(service)
        {
                        this.adjuntosAppService = adjuntosAppService;
        }

        //protected override void InitializePermission()
        //{
        //    this.InitializePermissionByDefault("Siniestro", "Elemento");
        //}


        [HttpGet]
        public async Task<IActionResult> GetAdjunto(Guid Id)
        {
            try
            {
                AdjuntosDto data = await Service.GetAdjunto(Id);
                return ReturnData<AdjuntosDto>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }


    }


 

}
