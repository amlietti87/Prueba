using Microsoft.AspNetCore.Authorization;
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
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    [Route("[controller]/[action]")]
    public class FdAccionesController : ManagerController<FdAcciones, int, FdAccionesDto, FdAccionesFilter, IFdAccionesAppService>
    {
        private readonly IAdjuntosAppService adjuntosAppService;

        public FdAccionesController(IFdAccionesAppService service, IAdjuntosAppService _adjuntosAppService)
            : base(service)
        {
            adjuntosAppService = _adjuntosAppService;
        }


        [HttpPost]
        public async Task<IActionResult> AplicarAccion([FromBody]AplicarAccioneDto dto)
        {
            try
            {
                var dynamic = await this.Service.AplicarAccion(dto);
                return ReturnData<Object>(dynamic);
            }
            catch (Exception ex)
            {
                return ReturnError<Object>(ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFiles([FromQuery] Guid id)
        {
            try
            {
                var file = await this.adjuntosAppService.GetByIdAsync(id);
                var content = new System.IO.MemoryStream(file.Archivo);
                var contentType = "APPLICATION/octet-stream";
                var fileName = file.Nombre;
                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);

            }

        }

    }




}
