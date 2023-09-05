using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using TECSO.FWK.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using TECSO.FWK.Caching;
using Microsoft.Net.Http.Headers;
using ROSBUS.Admin.Domain.Report;

namespace ROSBUS.WebService.ART.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class DenunciasController : ManagerSecurityController<ArtDenuncias, int, ArtDenunciasDto, DenunciasFilter, IDenunciasAppService>
    {

        private readonly ICacheManager _cacheManager;
        private readonly IAdjuntosAppService _adjuntosAppService;
        private readonly IReporterHttpAppService _reporterHttpAppService;
        public DenunciasController(IDenunciasAppService service,
            IAdjuntosAppService adjuntosAppService,
            ICacheManager cacheManager, IReporterHttpAppService reporterHttpAppService)
            : base(service)
        {
            _adjuntosAppService = adjuntosAppService;
            _cacheManager = cacheManager;
            _reporterHttpAppService = reporterHttpAppService;
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("ART", "Denuncia");
            this.PermissionContainer.Permissions.Remove(this.PermissionContainer.Permissions.FirstOrDefault(e => e.ActionName == PermissionContainerBase.GetPagedList));
            this.PermissionContainer.AddPermission(PermissionContainerBase.GetPagedList, "ART", "Denuncia", "Visualizar");
        }

        [HttpGet]
        public async Task<IActionResult> HistorialReclamosEmpleado(int EmpleadoId)
        {
            try
            {
                var historial = await this.Service.HistorialReclamosEmpleado(EmpleadoId);
                return ReturnData<List<HistorialReclamosEmpleado>>(historial);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarPlanilla(DenunciaImportadorFileFilter filter)
        {
            try
            {
                return ReturnData<List<ImportadorExcelDenuncias>>(await this.Service.RecuperarPlanilla(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<List<ImportadorExcelDenuncias>>(ex);
            }

        }

        [HttpGet]
        public async Task<IActionResult> HistorialDenunciasPorEstado(int EmpleadoId)
        {
            try
            {
                var historial = await this.Service.HistorialDenunciasPorEstado(EmpleadoId);
                return ReturnData<List<HistorialDenunciasPorEstado>>(historial);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> HistorialDenunciaPorPrestador(int EmpleadoId)
        {
            try
            {
                var historial = await this.Service.HistorialDenunciaPorPrestador(EmpleadoId);
                return ReturnData<List<HistorialDenuncias>>(historial);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet]
        public override Task<IActionResult> GetAllAsync(DenunciasFilter filter)
        {
            return base.GetAllAsync(filter);
        }

        [HttpGet("{DenunciaId}")]
        public async Task<IActionResult> GetAdjuntosDenuncias(int DenunciaId)
        {
            try
            {
                List<AdjuntosDto> data = await Service.GetAdjuntosDenuncias(DenunciaId);
                return ReturnData<List<AdjuntosDto>>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> UploadFiles(int DenunciaId)
        {
            try
            {
                var result = await _adjuntosAppService.AgregarAdjuntos(this.Request.Form.Files);

                await this.Service.AgregarAdjuntos(DenunciaId, result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> ImportWithTask()
        {
            try
            {

                var result = await this.Service.ImportWithTask();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Anular([FromBody] ArtDenunciasDto data)
        {
            try
            {
                var result = await this.Service.Anular(data);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileById(Guid Id)
        {
            try
            {

                await _adjuntosAppService.DeleteAsync(Id);

                await this.Service.DeleteFileById(Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        // Exportar Excel
        [HttpPost]
        public async Task<IActionResult> GetReporteExcel([FromBody] DenunciasFilter filter)
        {
            filter.PageSize = null;
            var items = await this.Service.GetItemsAsync(filter);
            var filterIds = new ExcelDenunciasFilter();
            int c = 0;
            foreach (var item in items)
            {
                if (c == 0)
                {
                    filterIds.Ids = item.Id.ToString();
                    c = 1;
                }
                else
                {
                    filterIds.Ids += "," + item.Id.ToString();
                }
            }


            return ReturnData<FileDto>(await this.Service.GetReporteExcel(filterIds));
        }
        // Importar Excel
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {

            try
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        // Viajar al AppService
                        ICollection<ImportadorExcelDenuncias> validatedDTO = await this.Service.UploadExcel(fileBytes);


                        var id = Guid.NewGuid().ToString();

                        await _cacheManager.GetCache<string, ICollection<ImportadorExcelDenuncias>>("ImportadorExcelDenuncias").SetAsync(id, validatedDTO);

                        return ReturnData<string>(id);
                    }
                }
                else
                {
                    throw new ArgumentException("Falta EL archivo");
                }
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> ImportarDenuncias([FromBody] DenunciaImportadorFileFilter input)
        {
            try
            {

                    await this.Service.ImportarDenuncias(input);
                    return ReturnData<string>("");
                

            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody]ArtDenunciasDto dto)
        {

            try
            {
                var datos = await this.Service.GetDatosReporte(dto);
                byte[] byteArray = await this._reporterHttpAppService.GenerarReporteGenerico(datos);
                return new FileContentResult(byteArray, new
                MediaTypeHeaderValue("application/pdf"))
                {
                    FileDownloadName = ("DenunciaNro-" + dto.NroDenuncia + ".pdf")
                };
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

    }

}
