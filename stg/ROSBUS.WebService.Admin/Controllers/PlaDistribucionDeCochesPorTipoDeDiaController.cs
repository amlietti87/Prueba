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
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Caching;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class PlaDistribucionDeCochesPorTipoDeDiaController : ManagerController<PlaDistribucionDeCochesPorTipoDeDia, int, PlaDistribucionDeCochesPorTipoDeDiaDto, PlaDistribucionDeCochesPorTipoDeDiaFilter, IPlaDistribucionDeCochesPorTipoDeDiaAppService>
    {
        private readonly ICacheManager cacheManager;

        public PlaDistribucionDeCochesPorTipoDeDiaController(IPlaDistribucionDeCochesPorTipoDeDiaAppService service, ICacheManager _cacheManag)
            : base(service)
        {
            cacheManager = _cacheManag;
        }


        [HttpGet]
        public async Task<IActionResult> RecuperarPlanilla(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            try
            {
                return ReturnData<List<HMediasVueltasImportadaDto>>(await this.Service.RecuperarPlanilla(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<PlaDistribucionDeCochesPorTipoDeDiaDto>(ex);
            }

        }



        [HttpPost]
        public async Task<IActionResult> uploadPlanillaIvu(IFormFileCollection file)
        {
            try
            {
                var archivosNecesarios = await this.Service.ValidateArchivos(file);
                if (archivosNecesarios)
                {
                    List<ImportarHorariosDto> importacion = await this.Service.Horarios(file);

                    var id = Guid.NewGuid().ToString();

                    await cacheManager.GetCache<string, List<ImportarHorariosDto>>("ImportarHorariosDto").SetAsync(id, importacion);

                    return ReturnData<string>(id);
                }
                else
                {
                    return null;
                }                
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
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

                        List<ImportarHorariosDto> importacion = new List<ImportarHorariosDto>();


                        for (int row = startRow; row <= end.Row; row++)
                        { // Row by row...
                            ImportarHorariosDto item = new ImportarHorariosDto();

                            var col = start.Column;
                            var cel = workSheet.Cells[row, col++];
                            item.Servicio = cel.Text;
                            

                            cel = workSheet.Cells[row, col++];
                            var valueSale = (cel.Value as double?);
                            item.Sale_EsDiaPosterior = valueSale >= 1;
                            this.FormatCell(cel, "hh:mm:ss");
                            item.Sale = cel.Text;
                            

                            cel = workSheet.Cells[row, col++];
                            var valueLlega = (cel.Value as double?);
                            item.Llega_EsDiaPosterior = valueLlega >= 1;
                            this.FormatCell(cel, "hh:mm:ss");
                            item.Llega = cel.Text;


                            item.TieneFormatoFechaInvalido = !(valueSale >= 0 && valueSale < 2 && valueLlega >= 0 && valueLlega < 2);

                            cel = workSheet.Cells[row, col++];
                            item.Bandera = cel.Text;

                            cel = workSheet.Cells[row, col++];
                            item.dsc_TpoHora = cel.Text;

                            cel = workSheet.Cells[row, col++];
                            item.des_subg = cel.Text;

                            
                            if (
                                !string.IsNullOrWhiteSpace(item.Servicio) &&
                                !string.IsNullOrWhiteSpace(item.Sale) &&
                                !string.IsNullOrWhiteSpace(item.Llega) &&
                                !string.IsNullOrWhiteSpace(item.Bandera) &&
                                !string.IsNullOrWhiteSpace(item.dsc_TpoHora) &&
                                !string.IsNullOrWhiteSpace(item.des_subg) 
                                )
                            {
                                importacion.Add(item);
                            }
                            
                        }

                        var id = Guid.NewGuid().ToString();

                        await cacheManager.GetCache<string, List<ImportarHorariosDto>>("ImportarHorariosDto").SetAsync(id, importacion);

                        return ReturnData<string>(id);
                    }
                }
                else {
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
        public async Task<IActionResult> ImportarServicios([FromBody] ImportarServiciosInput input)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.ImportarServicios(input);
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
        public async Task<IActionResult> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            try
            {
                var result = await this.Service.ExistenMediasVueltasIncompletas(input);
                return ReturnData<int?>((int?)result.Estado);
            }
            catch (Exception ex)
            {
                return ReturnError<int?>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> RecrearSabanaSector([FromBody] PlaDistribucionDeCochesPorTipoDeDia input)
        {
            try
            {
                await this.Service.RecrearSabanaSector(input);
                return ReturnData<string>(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            try
            {
                var result = await this.Service.TieneMinutosAsignados(filter);
                return ReturnData<bool>(result);
            }
            catch (Exception ex)
            {
                return ReturnError<bool>(ex);
            }
        }
        

    }

}
