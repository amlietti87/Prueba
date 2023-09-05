using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    //[Route("api/recibos/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class FirmadorController : ManagerController<FdDocumentosProcesados, long, FdDocumentosProcesadosDto, FdDocumentosProcesadosFilter, IFdDocumentosProcesadosAppService>
    {
        private readonly IFdFirmadorAppService fdFirmadorAppService;

        public FirmadorController(IFdDocumentosProcesadosAppService service, IAuthService authService, IFdFirmadorAppService fdFirmadorAppService)
            : base(service)
        {
            this.fdFirmadorAppService = fdFirmadorAppService;
        }

        [NonAction]
        public override Task<IActionResult> GetAllAsync(FdDocumentosProcesadosFilter filter)
        {
            return base.GetAllAsync(filter);
        }


        [NonAction]
        public override Task<IActionResult> GetByIdAsync(long Id, bool blockEntity = true)
        {
            return base.GetByIdAsync(Id, blockEntity);
        }

        [NonAction]
        public override Task<IActionResult> GetItemsAsync(FdDocumentosProcesadosFilter filter)
        {
            return base.GetItemsAsync(filter);
        }

        [NonAction]
        public override Task<IActionResult> GetPagedList([FromBody] FdDocumentosProcesadosFilter filter)
        {
            return base.GetPagedList(filter);
        }

        [NonAction]
        public override Task<IActionResult> UpdateEntity([FromBody] FdDocumentosProcesadosDto dto)
        {
            return base.UpdateEntity(dto);
        }

        [NonAction]
        public override Task<IActionResult> SaveNewEntity([FromBody] FdDocumentosProcesadosDto dto)
        {
            return base.SaveNewEntity(dto);
        }

        [NonAction]
        public override Task<IActionResult> DeleteById(long Id)
        {
            return base.DeleteById(Id);
        }

        [NonAction]
        public override Task<IActionResult> DeleteEntity([FromBody] FdDocumentosProcesadosFilter filter)
        {
            return base.DeleteEntity(filter);
        }


        [HttpGet]
        [Route("api/usuario/[action]")]
        public async Task<ActionResult> Certificado(string idUsuario)
        {
            FdFirmadorDto dto = null;
            try
            {
                var token = this.authService.GetCurretToken();

                dto = await this.Service.GetCertificado(token, idUsuario, Request);

                return File(dto.file, "application/x-pkcs12", String.Format("\"{0}\"", dto.FileName));

            }

            catch (ValidationException vex)
            {
                await LogError(vex);
                return this.UnprocessableEntity(new { error = vex.Message });
            }
            catch (FileNotFoundException fEx)
            {
                await LogError(fEx);
                return this.NotFound(new { error = fEx.Message });
            }
            catch (Exception ex)
            {
                await LogError(ex);
                return this.StatusCode(500, new { error = ex.Message });
            }

        }



        [HttpGet]
        [Route("api/recibos/[action]")]
        [ActionName("metadatos")]
        public async Task<IActionResult> Metadatos(string idUsuario)
        {
            FdFirmadorDto dto = null;
            try
            {
                var token = this.authService.GetCurretToken();

                dto = await this.Service.GetMetadatos(token, idUsuario, Request);

                return Ok(dto.Metadatos);

            }
            catch (ValidationException vex)
            {
                await LogError(vex);
                return this.UnprocessableEntity(new { error = vex.Message });
            }
            catch (Exception ex)
            {
                await LogError(ex);
                return this.StatusCode(500, new { error = ex.Message });
            }

        }


        [HttpGet]
        [Route("api/recibos/[action]")]
        public async Task<IActionResult> Descarga(int idRecibo)
        {
            FdFirmadorDto dto = null;
            try
            {
                var token = this.authService.GetCurretToken();

                dto = await this.Service.GetDocumento(token, idRecibo, Request);

                return File(dto.file, "application/pdf", String.Format("\"{0}\"", dto.FileName));
            }

            catch (ValidationException vex)
            {
                await LogError(vex);
                return this.UnprocessableEntity(new { error = vex.Message });
            }
            catch (InvalidDocumentExeption iEx)
            {
                await LogError(iEx);
                return this.StatusCode(StatusCodes.Status406NotAcceptable, new { error = iEx.Message });
            }
            catch (FileNotFoundException fEx)
            {
                await LogError(fEx);
                return this.StatusCode(StatusCodes.Status405MethodNotAllowed, new { error = fEx.Message });
            }
            catch (AccessViolationException aEx)
            {
                await LogError(aEx);
                return this.Conflict(new { error = aEx.Message });
            }

            catch (InvalidDataException invEx)
            {
                await LogError(invEx);
                return this.UnprocessableEntity(new { error = invEx.Message });
            }

            catch (Exception ex)
            {
                await LogError(ex);
                return this.StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/recibos/[action]")]
        public async Task<IActionResult> Firmado(int idRecibo, Boolean? conformidad)
        {
            FdFirmadorDto dto = null;
            try
            {
                var token = this.authService.GetCurretToken();

                dto = await this.Service.RecibirDocumento(token, idRecibo, conformidad, Request);

                return Ok();
            }

            catch (ValidationException vex)
            {
                await LogError(vex);
                return this.UnprocessableEntity(new { error = vex.Message });
            }
            catch (FileNotFoundException fEx)
            {
                await LogError(fEx);
                return this.StatusCode(StatusCodes.Status405MethodNotAllowed, new { error = fEx.Message });
            }
            catch (InvalidDocumentExeption iEx)
            {
                await LogError(iEx);
                return this.StatusCode(StatusCodes.Status406NotAcceptable, new { error = iEx.Message });
            }
            catch (InvalidDataException invEx)
            {
                await LogError(invEx);
                return this.UnprocessableEntity(new { error = invEx.Message });
            }
            catch (AccessViolationException aEx)
            {
                await LogError(aEx);
                return this.StatusCode(StatusCodes.Status409Conflict, new { error = aEx.Message });
            }
            catch (Exception ex)
            {
                await LogError(ex);
                return this.StatusCode(500, new { error = ex.Message });
            }
        }



    }


}
