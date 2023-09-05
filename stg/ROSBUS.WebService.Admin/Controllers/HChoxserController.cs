using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Caching;
using TECSO.FWK.Extensions;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HChoxserController : ManagerController<HChoxser, int, HChoxserDto, HChoxserFilter, IHChoxserAppService>
    {

        private readonly ICacheManager cacheManager;

        public HChoxserController(IHChoxserAppService service, ICacheManager _cacheManag)
            : base(service)
        {
            cacheManager = _cacheManag;
        }


        [HttpPost]
        public async Task<IActionResult> uploadPlanilla(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        ExcelPackage pck = new ExcelPackage(file.OpenReadStream());
                        var workSheet = pck.Workbook.Worksheets.FirstOrDefault(e=> e.Name== "Sist. Planificacion");

                        if (workSheet==null)
                        {
                            throw new ValidationException("El archivo debe contener una hoja llamada 'Sist. Planificacion'");
                        }

                        var start = workSheet.Dimension.Start;
                        var end = workSheet.Dimension.End;

                        var startRow = start.Row + 1;


                        //List<CabeceraExcelImportado> importacion = new List<CabeceraExcelImportado>();
                        ImportadorHChoxser importador = new ImportadorHChoxser();

                        for (int row = startRow; row <= end.Row; row++)
                        { // Row by row...

                            var col = start.Column;
                            ItemImportadorHChoxser item = new ItemImportadorHChoxser();
                            item.IsValid = true;
                            item.servicio = workSheet.Cells[row, col++].Text;
                            if (!string.IsNullOrEmpty(item.servicio) && item.servicio!="0")
                            {
                                try
                                {
                                    var cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.SaleDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.LlegaDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel,"hh:mm");
                                    item.duracion = cel.Text;

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.SaleRelevoDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.LlegaRelevoDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel,"hh:mm");
                                    item.duracionRelevo = cel.Text;

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.SaleAuxiliarDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.LlegaAuxiliarDate = cel.Text.ToDateTime();

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel,"hh:mm");
                                    item.duracionAuxiliar = cel.Text;

                                    cel = workSheet.Cells[row, col++];
                                    this.FormatCell(cel);
                                    item.tipoDeDia = cel.Text;

                                }
                                catch (Exception ex)
                                {
                                    item.IsValid = false;
                                    item.Errors.Add(ex.Message);
                                }
                                
                                
                                importador.List.Add(item);
                                
                            }
                        }

                        var id = Guid.NewGuid().ToString();

                        await cacheManager.GetCache<string, ImportadorHChoxser>("ItemImportadorHChoxser").SetAsync(id, importador);

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

        private void FormatCell(ExcelRange cel, string format= "dd/MM/yyyy h:mm")
        {
            if (cel.Style.Numberformat.Format != format)
            {
                cel.Style.Numberformat.Format = format;
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarPlanilla(HChoxserFilter filter)
        {
            try
            {
                return ReturnData<ImportadorHChoxserResult>(await this.Service.RecuperarPlanilla(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<ImportadorHChoxserResult>(ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> ImportarDuraciones([FromBody] HChoxserFilter input)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.ImportarDuraciones(input);
                    return ReturnData<string>("");
                }
                else
                {
                    return ReturnError<ImportarServiciosInput>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDurYSer([FromBody] HorarioDuracion horarioDuracion)
        {
            HChoxserExtendedDto Duracion = horarioDuracion.Duracion;
            HHorariosConfiDto Horario = horarioDuracion.Horario;

            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.UpdateDurYSer(Duracion, Horario);
                    return ReturnData<string>("");
                }
                else
                {
                    return ReturnError<ImportarServiciosInput>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> RecuperarDuraciones(HHorariosConfiFilter filter)
        {
            try
            {
                return ReturnData<List<HChoxserExtendedDto>>(await this.Service.RecuperarDuraciones(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<List<HChoxserExtendedDto>>(ex);
            }

        }

        


    }


 

}
