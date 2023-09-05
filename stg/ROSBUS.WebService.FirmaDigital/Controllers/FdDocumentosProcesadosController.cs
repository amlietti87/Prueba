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
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain;
using TECSO.FWK.Domain;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class FdDocumentosProcesadosController : ManagerController<FdDocumentosProcesados, long, FdDocumentosProcesadosDto, FdDocumentosProcesadosFilter, IFdDocumentosProcesadosAppService>
    {
        private readonly IDefaultEmailer _defaultEmailer;
        public FdDocumentosProcesadosController(IFdDocumentosProcesadosAppService service, IDefaultEmailer defaultEmailer)
            : base(service)
        {
            _defaultEmailer = defaultEmailer;
        }


        [HttpGet]
        public async Task<IActionResult> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter)
        {
            try
            {
                var historial = await this.Service.HistorialArchivosPorEstado(documentosProcesadosFilter);
                return ReturnData<List<ArchivosTotalesPorEstado>>(historial);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailDefault()
        {
            try
            {
                var email = await this.Service.GetEmailDefault();
                return ReturnData<string>(email);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportarExcel([FromBody]FdDocumentosProcesadosFilter filter)
        {
            try
            {
                var dynamic = await this.Service.ExportarExcel(filter);
                return ReturnData<FileDto>(dynamic);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidarEmails(string emails)
        {
            try
            {
                var dynamic = await _defaultEmailer.ValidateEmails(emails);
                if (!String.IsNullOrWhiteSpace(dynamic))
                {
                    throw new DomainValidationException(dynamic);
                }
                return ReturnData<String>(dynamic);
            }
            catch (Exception ex)
            {
                return ReturnError<String>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> ImportarDocumentos()
        {
            try
            {
                var result = await this.Service.ImportarDocumentos();
                return ReturnData<Boolean>(result);
            }
            catch (Exception ex)
            {
                return ReturnError<Boolean>(ex);
            }
        }
    }




}
