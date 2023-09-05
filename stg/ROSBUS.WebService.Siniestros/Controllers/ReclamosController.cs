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
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Caching;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class ReclamosController : ManagerSecurityController<SinReclamos, int, ReclamosDto, ReclamosFilter, IReclamosAppService>
    {
        private readonly IAdjuntosAppService adjuntosAppService;
        private readonly ICacheManager _cacheManager;

        public ReclamosController(IReclamosAppService service, 
                                  IAdjuntosAppService adjuntosAppService,
                                  ICacheManager cacheManager)
            : base(service)
        {
            this.adjuntosAppService = adjuntosAppService;
            _cacheManager = cacheManager;

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "Reclamo");
            this.PermissionContainer.Permissions.Remove(this.PermissionContainer.Permissions.FirstOrDefault(e => e.ActionName == PermissionContainerBase.GetPagedList));
            this.PermissionContainer.AddPermission(PermissionContainerBase.GetPagedList, "Siniestro", "Reclamo", "Visualizar");
            this.PermissionContainer.AddPermission(PermissionContainerBase.GetPagedList, "Reclamo", "Reclamo", "Visualizar");
            this.PermissionContainer.AddPermission("CambioEstado", "Siniestro", "Reclamo", "CambiaEstado");
        }


        [HttpGet("{ReclamoId}")]
        public async Task<IActionResult> GetAdjuntos(int ReclamoId)
        {
            try
            {
                List<AdjuntosDto> data = await Service.GetAdjuntos(ReclamoId);
                return ReturnData<List<AdjuntosDto>>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }
        public override Task<IActionResult> SaveNewEntity([FromBody] ReclamosDto dto)
        {
            dto.Fecha = dto.Fecha.Date;
            return base.SaveNewEntity(dto);
        }

        [HttpPost()]
        public async override Task<IActionResult> UpdateEntity([FromBody] ReclamosDto dto)
        {
            dto.Fecha = dto.Fecha.Date;
            return await base.UpdateEntity(dto);
        }

        [HttpPost()]
        public async Task<IActionResult> CambioEstado([FromBody]PostReclamo data)
        {
            try
            {
                var resul = await Service.CambioEstado(data.Reclamo, data.Historico);
                return ReturnData<ReclamosDto>(resul);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> UploadFiles(int ReclamoId)
        {

            try
            {
                var result = await adjuntosAppService.AgregarAdjuntos(this.Request.Form.Files);

                await this.Service.AgregarAdjuntos(ReclamoId, result);

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

                await adjuntosAppService.DeleteAsync(Id);

                await this.Service.DeleteFileById(Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Anular([FromBody] ReclamosDto data)
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
        public async Task<Boolean> CheckNuevoReclamoNoNecesario([FromBody] ReclamosFilter filter)
        {
            try
            {
                var result = await this.Service.CheckNuevoReclamoNoNecesario(filter);

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetReporteExcel([FromBody] ReclamosFilter filter)
        {
            return ReturnData(await Service.GetReporteExcel(filter));
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

                        ICollection<ImportadorExcelReclamos> validatedDTO = await Service.UploadExcel(fileBytes);


                        var id = Guid.NewGuid().ToString();

                        await _cacheManager.GetCache<string, ICollection<ImportadorExcelReclamos>>("ImportadorExcelReclamos").SetAsync(id, validatedDTO);

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

        [HttpGet]
        public async Task<IActionResult> RecuperarPlanilla(ReclamoImportadorFileFilter filter)
        {
            try
            {
                return ReturnData(await Service.RecuperarPlanilla(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<List<ImportadorExcelReclamos>>(ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> ImportarReclamos([FromBody] ReclamoImportadorFileFilter input)
        {
            try
            {

                await Service.ImportarReclamos(input);
                return ReturnData<string>("");


            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

    }

    public class PostReclamo
    {
        public ReclamosDto Reclamo { get; set; }
        public ReclamosHistoricosDto Historico { get; set; }
    }



}
