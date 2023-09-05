using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HHorariosConfiController : ManagerController<HHorariosConfi, int, HHorariosConfiDto, HHorariosConfiFilter, IHHorariosConfiAppService>
    {
        public HHorariosConfiController(IHHorariosConfiAppService service)
            : base(service)
        {

        }



        [HttpPost]
        public async Task<IActionResult> ReporteDistribucionCoches([FromBody]ReporteDistribucionCochesFilter filter)
        {
            FileDto stream = await this.Service.ReporteDistribucionCoches(filter);
            return ReturnData<FileDto>(stream);
        }


        [HttpPost]
        public async Task<IActionResult> ReporteParadasPasajeros([FromBody]ReportePasajerosFilter filter)
        {
            try
            {
                FileDto stream = await this.Service.ReporteParadasPasajeros(filter);
                return ReturnData<FileDto>(stream);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto>(ex);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> ReporteDistribucionCochesGet(ReporteDistribucionCochesFilter filter)
        {
            var stream = (await this.Service.ReporteDistribucionCoches(filter));


            return File(
                stream.ByteArray,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "report.xlsx");
        }




        [HttpPost]
        public async Task<IActionResult> ReporteDetalleSalidasYRelevos([FromBody]DetalleSalidaRelevosFilter filter)
        {
            FileDto stream = (await this.Service.ReporteDetalleSalidasYRelevos(filter));

            return ReturnData<FileDto>(stream);
        }

        [HttpGet]
        public async Task<IActionResult> ReporteDetalleSalidasYRelevosGet(DetalleSalidaRelevosFilter filter)
        {
            FileDto stream = (await this.Service.ReporteDetalleSalidasYRelevos(filter));
            return File(
                 stream.ByteArray,
                 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 "report.xlsx");
        }


        [HttpPost]
        public async Task<IActionResult> ReporteHorarioPasajeros([FromBody]ReporteHorarioPasajerosFilter filter)
        {
            FileDto stream = (await this.Service.ReporteHorarioPasajeros(filter));

            return ReturnData<FileDto>(stream);
        }


        public override async Task<IActionResult> UpdateEntity([FromBody] HHorariosConfiDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HHorariosConfiDto nedto = await this.Service.UpdateAsync(dto);
                    return ReturnData<HHorariosConfiDto>(nedto);
                }
                else
                {
                    return ReturnError<HHorariosConfiDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<HHorariosConfiDto>(ex);
            }

        }



        [HttpPost]
        public virtual async Task<IActionResult> DeleteDuracionesServicio(int IdServicio)
        {
            try
            {
                await this.Service.DeleteDuracionesServicio(IdServicio);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        public override async Task<IActionResult> SaveNewEntity([FromBody] HHorariosConfiDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HHorariosConfiDto nedto = await this.Service.AddAsync(dto);
                    return ReturnData<HHorariosConfiDto>(nedto);
                }
                else
                {
                    return ReturnError<HHorariosConfiDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<HHorariosConfiDto>(ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> UpdateCantidadCochesReales([FromBody] HHorariosConfiDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Boolean rta = await this.Service.UpdateCantidadCochesReales(dto);
                    return ReturnData<Boolean>(rta);
                }
                else
                {
                    return ReturnError<HHorariosConfiDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<HHorariosConfiDto>(ex);
            }

        }







    }




}
