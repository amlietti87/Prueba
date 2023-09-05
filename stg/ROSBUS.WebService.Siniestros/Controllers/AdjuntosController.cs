using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    [Authorize]
    public class AdjuntosController : TECSO.FWK.ApiServices.ControllerBase
    {
        private IAdjuntosAppService _service;

        public AdjuntosController(IAdjuntosAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFiles([FromQuery] Guid id)
        {
            try
            {
                var file = await this._service.GetByIdAsync(id);
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


        [HttpPost()]
        public async Task<IActionResult> UploadFiles(IFormFileCollection files)
        {

            try
            {
                if (files.Count == 0)
                {
                    var r = this.Request.Form;
                    var r2 = r.Files.ToList();
                    files = r.Files;
                }

                var result = await this._service.AgregarAdjuntos(this.Request.Form.Files);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> UploadOrUpdateFile(IFormFileCollection files, Guid? Id)
        {

            try
            {
                var file = this.Request.Form.Files.FirstOrDefault();
                var result = await this._service.UploadOrUpdateFile(file, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }



        [HttpPost]
        public virtual async Task<IActionResult> DeleteById(Guid Id)
        {
            try
            {
                await this._service.DeleteAsync(Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

    }




}
