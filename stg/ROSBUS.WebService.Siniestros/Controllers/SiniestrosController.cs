using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Operaciones.AppService.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using ReportModel = ROSBUS.Admin.Domain.Report.ReportModel;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class SiniestrosController : ManagerSecurityController<SinSiniestros, int, SiniestrosDto, SiniestrosFilter, ISiniestrosAppService>
    {
        private readonly IAdjuntosAppService adjuntosAppService;
        private readonly IEmpleadosAppService _empleadosAppService;
        private readonly ILocalidadesAppService _localidaddervice;
        private readonly ICroCroquisAppService _croCroquisAppService;
        private readonly IReporterHttpAppService _reporterHttpAppService;
        public SiniestrosController(ISiniestrosAppService service, IReporterHttpAppService reporterHttpAppService, IAdjuntosAppService adjuntosAppService, IEmpleadosAppService empleadosAppService, ILocalidadesAppService localidaddervice, ICroCroquisAppService croCroquisAppService)
            : base(service)
        {
            this.adjuntosAppService = adjuntosAppService;
            this._empleadosAppService = empleadosAppService;
            this._localidaddervice = localidaddervice;
            this._croCroquisAppService = croCroquisAppService;
            this._reporterHttpAppService = reporterHttpAppService;
        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "Siniestro");
            this.PermissionContainer.Permissions.Remove(this.PermissionContainer.Permissions.FirstOrDefault(e => e.ActionName == PermissionContainerBase.GetPagedList));
            this.PermissionContainer.AddPermission(PermissionContainerBase.GetPagedList, "Siniestro", "Siniestro", "Visualizar");
        }


        [HttpGet]
        public async Task<IActionResult> GetHistorialEmpPract(bool empleado, int id)
        {
            try
            {
                var data = await Service.GetHistorialEmpPract(empleado, id);
                return ReturnData<HistorialSiniestros>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<HistorialSiniestros>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNroSiniestroMax()
        {
            try
            {
                var data = await Service.GetNroSiniestroMax();
                return ReturnData<int>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost]
        public override async Task<IActionResult> UpdateEntity([FromBody] SiniestrosDto dto)
        {
            dto.ConductorEmpresa = null;
            dto.Empresa = null;


            try
            {
                if (ModelState.IsValid)
                {
                    int id = (await this.Service.UpdateAsync(dto)).Id;
                    return ReturnData<int>(id);
                }
                else
                {
                    return ReturnError<SiniestrosDto>(this.ModelState);
                }
                
            }
            catch (ValidationException ex)
            {
                if (ex.Message== "inf_informesERROR")
                {
                    var messages = new List<string>() { "Ha ocurrido un error al generar el informe." };

                    return ReturnData<int>(dto.Id,ActionStatus.Ok, messages);
                }

                return ReturnError<SiniestrosDto>(ex);

            }

            
        }

        [HttpPost]
        public override async Task<IActionResult> SaveNewEntity([FromBody] SiniestrosDto dto)
        {
            dto.ConductorEmpresa = null;
            dto.Empresa = null;
            dto.FechaDenuncia = DateTime.Now;
            int id = 0;
            try
            {
                if (this.ModelState.IsValid)
                {
                    id = (await this.Service.AddAsync(dto)).Id;

                    if (dto.GenerarInforme.HasValue && dto.GenerarInforme.Value)
                    {
                        await this.Service.GenerarInforme(id);
                    }

                    return ReturnData<int>(id);
                }
                else
                {
                    return ReturnError<SiniestrosDto>(this.ModelState);
                }
                
            }
            catch (ValidationException ex)
            {
                if (ex.Message == "inf_informesERROR")
                {
                    var messages = new List<string>() { "Ha ocurrido un error al generar el informe." };

                    return ReturnData<int>(id, ActionStatus.Ok, messages);
                }

                return ReturnError<SiniestrosDto>(ex);

            }
            
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody]SiniestrosDto dto)
        {

            try
            {
                if (dto.CroquiId.HasValue)
                {
                    var result = await _croCroquisAppService.GetByIdAsync(dto.CroquiId.Value);

                    var croquibase64 = await this._reporterHttpAppService.GenerarBase64DesdeSvg(result.Svg);

                    dto.CroquiBase64 = croquibase64;

                }


                var datos = await this.Service.GetDatosReporte(dto);


                var byteArray = await this._reporterHttpAppService.GenerarReporteGenerico(datos);

                return new FileContentResult(byteArray, new
                MediaTypeHeaderValue("application/pdf"))
                {
                    FileDownloadName = ("SiniestroNro-" + dto.NroSiniestro + ".pdf")
                };
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }
        [HttpPost]
        

        [HttpPost]
        public async Task<IActionResult> GenerateReportById([FromBody] int Id)
        {

           var dto = await this.Service.GetDtoByIdAsync(Id);

            try
            {
                if (dto.CroquiId.HasValue)
                {
                    var result = await _croCroquisAppService.GetByIdAsync(dto.CroquiId.Value);

                    var croquibase64 = await this._reporterHttpAppService.GenerarBase64DesdeSvg(result.Svg);

                    dto.CroquiBase64 = croquibase64;

                }


                var datos = await this.Service.GetDatosReporte(dto);


                var byteArray = await this._reporterHttpAppService.GenerarReporteGenerico(datos);

                return new FileContentResult(byteArray, new
                MediaTypeHeaderValue("application/pdf"))
                {
                    FileDownloadName = ("SiniestroNro-" + dto.NroSiniestro + ".pdf")
                };
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet("{SiniestroId}")]
        public async Task<IActionResult> GetAdjuntosSiniestros(int SiniestroId)
        {
            try
            {
                List<AdjuntosDto> data = await Service.GetAdjuntosSiniestros(SiniestroId);
                return ReturnData<List<AdjuntosDto>>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> UploadFiles(int SiniestroId)
        {
            try
            {
                var result = await adjuntosAppService.AgregarAdjuntos(this.Request.Form.Files);

                await this.Service.AgregarAdjuntos(SiniestroId, result);

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

        [HttpGet]
        public override Task<IActionResult> GetByIdAsync(int Id, bool blockEntity = true)
        {
            return base.GetByIdAsync(Id, blockEntity);
        }

    }




}
