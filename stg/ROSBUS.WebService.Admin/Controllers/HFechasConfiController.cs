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
using TECSO.FWK.ApiServices.Model;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HFechasConfiController : ManagerController<HFechasConfi, int, HFechasConfiDto, HFechasConfiFilter, IHFechasConfiAppService>
    {

        private readonly IReporterHttpAppService _reporterHttpAppService;

        public HFechasConfiController(IHFechasConfiAppService service, IReporterHttpAppService reporterHttpAppService)
            : base(service)
        {
            this._reporterHttpAppService = reporterHttpAppService;
        }

        [HttpPost]
        public async override Task<IActionResult> UpdateEntity([FromBody] HFechasConfiDto dto)
        {
            try
            {
                var hfecha = await this.Service.UpdateAsync(dto);
                return ReturnData<HFechasConfiDto>(hfecha);
            }
            catch (Exception ex)
            {

                return ReturnError<HFechasConfiDto>(ex);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetLineasHorarias()
        {
            try
            {

                var pList = await this.Service.GetLineasHorarias();

                return ReturnData<List<PlaHorarioFechaLineaListView>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<List<PlaHorarioFechaLineaListView>>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CopiarHorario([FromBody] HFechasConfiFilter filter)
        {
            try
            {

                ItemDto pList = await this.Service.CopiarHorario(filter.cod_hfecha.Value, filter.fec_desde.Value, filter.CopyConductores.Value);

                return ReturnData<ItemDto>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<List<PlaHorarioFechaLineaListView>>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHBasec([FromBody] HBasecDto hbasec)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HBasecDto rta = await this.Service.UpdateHBasec(hbasec);
                    return ReturnData<HBasecDto>(rta);
                }
                else
                {
                    return ReturnError<HBasecDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<HBasecDto>(ex);
            }
        }


        [HttpGet]
        public async Task<ResponseModel<Boolean>> HorarioDiagramado(int CodHfecha, int? idServicio)
        {
            try
            {
                List<int> servicios = null;

                if (idServicio.HasValue)
                {
                    servicios = new List<int>() { idServicio.Value };
                }

                var result = await this.Service.HorarioDiagramado(CodHfecha, null , servicios);

                return ResponseModel<Boolean>(result);

            }
            catch (Exception ex)
            {
                return ResponseModelError<Boolean>(ex);
            }
        }



        [HttpPost]
        public async Task<IActionResult> RemapearRecoridoBandera([FromBody] HFechasConfiFilter filter)
        {
            try
            {
                int result = await this.Service.RemapearRecoridoBandera(filter);
                return ReturnData<int>(result);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost]
        public override Task<IActionResult> DeleteById(int Id)
        {
            return base.DeleteById(Id);
        }

        [HttpPost]
        public async Task<IActionResult> GenerarExcelHorarios([FromBody]ExportarExcelFilter filter)
        {
            try
            {
                return ReturnData<FileDto>(await this.Service.GenerarExcelHorarios(filter));
            }
            catch (Exception ex)
            {

                return ReturnError<int>(ex);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReporteRelevo([FromBody]ExportarExcelFilter data)
        {
            try
            {
                var datos = await this.Service.GetDatosReporteRelevos(data);

                var titulo = await this.Service.GetTitulo(data);

                var byteArray = await this._reporterHttpAppService.GenerarReporteGenerico(datos);

                FileDto file = new FileDto();

                file.ByteArray = byteArray;
                file.ForceDownload = true;
                file.FileType = "application/pdf";
                file.FileName = titulo;
                file.FileDescription = "Reporte Relevos";
                return ReturnData<FileDto>(file);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpPost]
        
        public async Task<IActionResult> GuardarBanderaPorSerctor([FromBody]HFechasConfiDto data)
        {
            try
            {
                await this.Service.GuardarBanderaPorSector(data);
                return ReturnData<HFechasConfiDto>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }




    }




}
