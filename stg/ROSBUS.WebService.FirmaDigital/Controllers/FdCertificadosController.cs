using Microsoft.AspNetCore.Http;
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
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class FdCertificadosController : ManagerSecurityController<FdCertificados, int, FdCertificadosDto, FdCertificadosFilter, IFdCertificadosAppService>
    {
        private readonly IAuthAppService _authAppService;
        private readonly IAdjuntosAppService _adjuntosAppService;

        public FdCertificadosController(IFdCertificadosAppService service, IAuthAppService authAppService, IAdjuntosAppService adjuntosAppService)
            : base(service)
        {
            this._authAppService = authAppService;
            this._adjuntosAppService = adjuntosAppService;
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("FirmaDigital", "AdministrarCertificados");
        }

        [HttpGet]
        public async Task<IActionResult> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter)
        {
            try
            {
                var historial = await this.Service.HistorialCertificadosPorUsuario(fdCertificadosFilter);
                return ReturnData<List<FdCertificadosDto>>(historial);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> SubirCertificado(IFormFileCollection files)
        {
        

            try
            {
                if (files.Count == 0)
                {
                    var r = this.Request.Form;
                    var r2 = r.Files.ToList();
                    files = r.Files;
                }

                var result = await this._adjuntosAppService.AgregarAdjuntos(this.Request.Form.Files);

                var UserID = Convert.ToInt32( this.Request.Form["UsuarioId"].FirstOrDefault());

                var newCertificate = new FdCertificadosDto();
                newCertificate.UsuarioId = UserID;
                newCertificate.ArchivoId = result[0].Id;
                newCertificate.FechaActivacion = DateTime.Now;
                newCertificate.Activo = true;

                var certificate = await this.Service.AddAsync(newCertificate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RevocarCertificado([FromBody] FdCertificadosFilter filter) {

            try
            {
                var certificadoRevocado = await this.Service.RevocarCertificado(filter);
                return ReturnData<FdCertificadosDto>(certificadoRevocado);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> searhCertificate([FromBody] FdCertificadosFilter filter)
        {

            filter.UsuarioId = this.authService.GetCurretUserId();

            try
            {
                var certificadoActivoDto = await this.Service.GetDtoAllFilterAsync(filter);

                if (certificadoActivoDto != null)
                {
                    foreach (var item in certificadoActivoDto.Items)
                    {
                        var arch = await _adjuntosAppService.GetByIdAsync(item.ArchivoId);
                        item.ArchivoNombre = arch.Nombre;
                    }
                }
                return ReturnData <PagedResult<FdCertificadosDto>>(certificadoActivoDto);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> downloadCertificate([FromBody] FdCertificadosFilter filter)
        {


            
            try
            {
                var fileDTO = await this.Service.downloadCertificate(filter);
                return ReturnData<FileDto>(fileDTO);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> sendCertificateByEmail([FromBody] FdCertificadosFilter userFilter) {
            try
            {
                var fileDTO = await this.Service.sendCertificateByEmail(userFilter);
                return ReturnData<string>("OK");
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }
        }

    }


 

}
