using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
using TECSO.FWK.Caching;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HMinxtipoController : ManagerController<HMinxtipo, int, HMinxtipoDto, HMinxtipoFilter, IHMinxtipoAppService>
    {

        private readonly ICacheManager cacheManager;


        public HMinxtipoController(IHMinxtipoAppService service, ICacheManager _cacheManag)
            : base(service)
        {
            cacheManager = _cacheManag;

        }


        [HttpGet]
        public async virtual Task<IActionResult> GetMinutosPorSector(HMinxtipoFilter filter)
        {
            try
            {
                MinutosPorSectorDto pList = await this.Service.GetMinutosPorSector(filter);

                return ReturnData<MinutosPorSectorDto>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<MinutosPorSectorDto>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerarExcelMinXSec([FromBody]HMinxtipoFilter filter)
        {
            try
            {
                return ReturnData<List<FileDto>>(await this.Service.GenerarExcelMinXSec(filter));
            }
            catch (Exception ex)
            {

                return ReturnError<int>(ex);
            }

        }

        [HttpGet]
        public async virtual Task<IActionResult> GetHSectores(HMinxtipoFilter filter)
        {
            try
            {
                List<HSectoresDto> pList = await this.Service.GetHSectores(filter);

                return ReturnData<List<HSectoresDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<List<HSectoresDto>>(ex);
            }
        }


        [HttpPost]
        public async virtual Task<IActionResult> SetHSectores([FromBody] List<HSectoresDto> input)
        {
            try
            {
                List<HSectoresDto> pList = await this.Service.SetHSectores(input);

                return ReturnData<List<HSectoresDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<List<HSectoresDto>>(ex);
            }
        }


        [HttpPost]
        public async virtual Task<IActionResult> UpdateHMinxtipo([FromBody] List<HMinxtipoDto> input)
        {
            try
            {
                await this.Service.UpdateHMinxtipo(input);

                return ReturnData<string>("");

            }
            catch (Exception ex)
            {
                return ReturnError<List<HMinxtipoDto>>(ex);
            }
        }







        [HttpGet]
        public async Task<IActionResult> RecuperarPlanilla(HMinxtipoFilter filter)
        {
            try
            {
                return ReturnData<ImportadorHMinxtipoResult>(await this.Service.RecuperarPlanilla(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<ImportadorHMinxtipoResult>(ex);
            }

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
                        var workSheet = pck.Workbook.Worksheets.FirstOrDefault();


                        var start = workSheet.Dimension.Start;
                        var end = workSheet.Dimension.End;

                        var startRow = start.Row + 1;


                        //List<CabeceraExcelImportado> importacion = new List<CabeceraExcelImportado>();
                        ImportadorHMinxtipo importador = new ImportadorHMinxtipo();


                        int OrdenSector = 1;

                        for (int col = 5; col < end.Column + 1; col++)
                        {
                            if (!string.IsNullOrWhiteSpace(workSheet.Cells[1, col].Text))
                            {
                                var s = new SectorImportador();
                                s.Descripcion = workSheet.Cells[1, col].Text;
                                s.Orden = OrdenSector++;

                                importador.Sectores.Add(s);
                            }
                            
                        }



                        for (int row = startRow; row <= end.Row; row++)
                        { // Row by row...

                            var col = start.Column;

                            if (!string.IsNullOrWhiteSpace(workSheet.Cells[row, col].Text))
                            {
                                
                                CabeceraExcelImportado cabecera = new CabeceraExcelImportado();
                                cabecera.TipoDia = workSheet.Cells[row, col++].Text;
                                cabecera.Bandera = workSheet.Cells[row, col++].Text;
                                cabecera.TipoHora = workSheet.Cells[row, col++].Text;
                                cabecera.TotalMin = workSheet.Cells[row, col++].Text;



                                for (int orden = 1; orden < importador.Sectores.Count + 1; orden++)
                                {
                                    DetalleExcelImportado item = new DetalleExcelImportado();
                                    var cell = workSheet.Cells[row, col++];
                                    this.FormatCell(cell, "hh:mm:ss");
                                    item.Minuto = cell.Text;
                                    item.Orden = orden;
                                    item.Sector = importador.Sectores.FirstOrDefault(e => e.Orden == orden);
                                    cabecera.Detalle.Add(item);
                                }

                                importador.Cabeceras.Add(cabecera);
                            }
                            
                        }

                        var id = Guid.NewGuid().ToString();

                        await cacheManager.GetCache<string, ImportadorHMinxtipo>("CabeceraExcelImportado").SetAsync(id, importador);

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


        private void FormatCell(ExcelRange cel, string format = "dd/MM/yyyy h:mm")
        {
            if (cel.Style.Numberformat.Format != format)
            {
                cel.Style.Numberformat.Format = format;
            }
        }


        [HttpPost]
        public async virtual Task<TECSO.FWK.ApiServices.Model.ResponseModel<string>> CopiarMinutos([FromBody] CopiarHMinxtipoInput input)
        {
            try
            {
                var rsult = await Service.CopiarMinutosAsync(input);

                return ResponseModel<string>(rsult);

            }
            catch (Exception ex)
            {
                return ResponseModelError<string>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportarMinutos([FromBody] HMinxtipoImporarInput input)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.ImportarMinutos(input);
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

    }

}
