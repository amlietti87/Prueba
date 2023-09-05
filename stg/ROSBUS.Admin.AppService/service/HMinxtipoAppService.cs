using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HMinxtipoAppService : AppServiceBase<HMinxtipo, HMinxtipoDto, int, IHMinxtipoService>, IHMinxtipoAppService
    {
        private ITiposDeDiasService _tiposDeDiasService;
        private IBanderaService _banderaService;
        private IHFechasConfiService _hfechasConfiService;
        private ILineaService _lineaService;
        public HMinxtipoAppService(IHMinxtipoService serviceBase, IBanderaService banderaService, ITiposDeDiasService tiposDeDiasService, IHFechasConfiService hFechasConfiService, ILineaService lineaService) 
            :base(serviceBase)
        {
            this._banderaService = banderaService;
            this._tiposDeDiasService = tiposDeDiasService;
            this._lineaService = lineaService;
            this._hfechasConfiService = hFechasConfiService;
        }

        public async Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input)
        {
            return await this._serviceBase.CopiarMinutosAsync(input);
        }

        public async Task<List<HSectoresDto>> GetHSectores(HMinxtipoFilter filter) {
            var sectores = await this._serviceBase.GetHSectores(filter);
            return this.MapList<HSectores, HSectoresDto>(sectores).OrderBy(e => e.Orden).ToList();
        }

        public async Task<MinutosPorSectorDto> GetMinutosPorSector(HMinxtipoFilter filter)
        {
            var minutos = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
            
            var minutosdto = this.MapList<HMinxtipo, HMinxtipoDto>(minutos.Items).ToList();

            MinutosPorSectorDto result = new MinutosPorSectorDto();
            result.Minutos = minutosdto.OrderBy(e=> e.TipoHora).ThenBy(e=> e.TotalMin).ToList();


            var sectores = await this._serviceBase.GetHSectores(filter);

            result.Sectores = this.MapList<HSectores, HSectoresDto>(sectores).OrderBy(e => e.Orden).ToList();


            foreach (var item in result.Minutos)
            {
                foreach (var hDetaminxtipo in item.HDetaminxtipo)
                {
                    var sec = result.Sectores.FirstOrDefault(e => e.CodHsector == hDetaminxtipo.CodHsector);
                    if (sec!=null)
                        hDetaminxtipo.Orden = result.Sectores.FirstOrDefault(e => e.CodHsector == hDetaminxtipo.CodHsector).Orden;
                    else
                    {
                        throw new ValidationException("No se puede determinar el CodHsector " + hDetaminxtipo.CodHsector);
                    }
                }

                foreach (var sectorDto in result.Sectores)
                {
                    if (!item.HDetaminxtipo.Any(e=> e.CodHsector==sectorDto.CodHsector))
                    {
                        var deta = new HDetaminxtipoDto();
                        deta.CodHsector = sectorDto.CodHsector;
                        deta.CodMinxtipo = item.Id;
                        deta.Minuto = 0;
                        deta.Orden = sectorDto.Orden;
                        deta.IsNew = true;
                        item.HDetaminxtipo.Add(deta);
                    }
                }

                TimeSpan ts = new TimeSpan();

                foreach (var det in item.HDetaminxtipo)
                {
                    int min = (int)det.Minuto.GetValueOrDefault();
                    int segundos = (int)((det.Minuto.GetValueOrDefault() - min) * 100); 

                    ts = ts.Add(TimeSpan.FromMinutes(Convert.ToDouble(min)));
                    ts = ts.Add(TimeSpan.FromSeconds(Convert.ToDouble(segundos)));
                }

                item.Suma = Convert.ToDecimal(ts.TotalMinutes);

                item.HDetaminxtipo = item.HDetaminxtipo.OrderBy(e => e.Orden).ToList();
            }

            return result;

        }


        public async Task ImportarMinutos(HMinxtipoImporarInput input)
        {
            await this._serviceBase.ImportarMinutos(input);
        }

        public async Task<ImportadorHMinxtipoResult> RecuperarPlanilla(HMinxtipoFilter filter)
        {
            return await this._serviceBase.RecuperarPlanilla(filter);
        }

        public  async Task<List<HSectoresDto>> SetHSectores(List<HSectoresDto> input)
        {
            var entities = this.MapList<HSectoresDto, HSectores>(input);

            await this._serviceBase.SetHSectores(entities);

            var result = this.MapList<HSectores, HSectoresDto>(entities).ToList();
            return result;
        }

        public async Task UpdateHMinxtipo(List<HMinxtipoDto> input)
        {
            HMinxtipoFilter filter = new HMinxtipoFilter();
            //filter.CodBan = input.First().CodBan;
            //filter.CodTdia = input.First().CodTdia;
            //filter.CodHfecha = input.First().CodHfecha;
            //filter.TipoHora = input.First().TipoHora;
            filter.FilterIds = input.Select(e => e.Id).ToList();

            if (!filter.FilterIds.Any())
            {
                throw new ValidationException("Es requerido al menos un minuto para guardar");
            }

            var minutosporsectormodificados = new List<HMinxtipo>();

            var minutos = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
            
            foreach (var m in minutos.Items)
            {
                var Itemdto = input.Where(e => e.Id == m.Id).First();
                if (hasChange(Itemdto,m))
                {
                    minutosporsectormodificados.Add(this.MapObject(Itemdto, m));

                }
            }

            if (minutosporsectormodificados.Any())
            {
                await this._serviceBase.UpdateHMinxtipo(minutosporsectormodificados);
            }
            
        }

        private bool hasChange(HMinxtipoDto itemdto, HMinxtipo m)
        {
            var hasChange =  itemdto.HDetaminxtipo.Join(m.HDetaminxtipo
                                    , d => d.CodHsector
                                    , h => h.CodHsector
                                    , (d, e) => new { D = d, E = e }).Where(w =>w.D.IsNew ||  w.D.Minuto != w.E.Minuto).Any();

            var hasNew = itemdto.HDetaminxtipo.Any(e => e.IsNew);

            return hasChange || hasNew;
        }

        public async Task<List<FileDto>> GenerarExcelMinXSec(HMinxtipoFilter filter)
        {

            List<FileDto> files = new List<FileDto>();

            List<HTipodia> tiposdedias = new List<HTipodia>();
            if (filter.CodTdia == null)
            {
                tiposdedias = (await this._tiposDeDiasService.GetAllAsync(e => true)).Items.ToList();
            }
            else
            {
                tiposdedias = new List<HTipodia>() { (await this._tiposDeDiasService.GetByIdAsync(Convert.ToInt32(filter.CodTdia))) };
            }


            var fecha = await this._hfechasConfiService.GetByIdAsync(Convert.ToInt32(filter.CodHfecha));
            var linea = await this._lineaService.GetByIdAsync(fecha.CodLin);

            //Fijarse como armar varios archivos si quieren todas las banderas.
            foreach (int banderaId in filter.BanderasId)
            {

                filter.CodBan = banderaId;

                var bandera = await this._banderaService.GetByIdAsync(Convert.ToInt32(banderaId));
                
                


                var file = new FileDto();

                ExcelPackage excel = new ExcelPackage();
                foreach (HTipodia tp in tiposdedias)
                {
                    filter.CodTdia = tp.Id;
                    var minutos = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
                    var minutosdto = this.MapList<HMinxtipo, HMinxtipoDto>(minutos.Items).ToList();
                    MinutosPorSectorDto result = new MinutosPorSectorDto();
                    result.Minutos = minutosdto;
                    
                    var sectores = await this._serviceBase.GetHSectores(filter);
                    result.Sectores = this.MapList<HSectores, HSectoresDto>(sectores).OrderBy(e => e.Orden).ToList();

                    var minutoxsecagrup = result.Minutos.GroupBy(e => new { CodTdia = e.CodTdia, TotalMin = e.TotalMin, TipoHoraDesc = e.TipoHoraDesc });
                    var hoja = minutoxsecagrup.FirstOrDefault(e => e.Key.CodTdia == tp.Id);

                    if (hoja != null)
                    {
                        var sheets = excel.Workbook.Worksheets.Add(tp.DesTdia);
                        int row = 1;
                        var cellheder = 1;

                        sheets.Cells[row, cellheder].Style.Font.Bold = true;
                        sheets.Cells[row, cellheder].Style.Font.Size = 11;
                        sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder++].Value = "Tipo de Dia";
                        sheets.Cells[row, cellheder].Style.Font.Bold = true;
                        sheets.Cells[row, cellheder].Style.Font.Size = 11;
                        sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder++].Value = "Bandera";
                        sheets.Cells[row, cellheder].Style.Font.Bold = true;
                        sheets.Cells[row, cellheder].Style.Font.Size = 11;
                        sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder++].Value = "Tipo de Hora";
                        sheets.Cells[row, cellheder].Style.Font.Bold = true;
                        sheets.Cells[row, cellheder].Style.Font.Size = 11;
                        sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        sheets.Cells[row, cellheder++].Value = "Tiempo de MV";

                        foreach (var sector in result.Sectores.OrderBy(e => e.Orden))
                        {
                            sheets.Cells[row, cellheder].Style.Font.Bold = true;
                            sheets.Cells[row, cellheder].Style.Font.Size = 11;
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder++].Value = sector.Calle1 + "-" + sector.Calle2;

                        }

                        var data = minutoxsecagrup.Where(e => e.Key.CodTdia == tp.Id);

                        foreach (var item in data.OrderBy(e => e.Key.TotalMin).ThenBy(e => e.Key.TipoHoraDesc))
                        {
                            row = row + 1;
                            cellheder = 1;
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder++].Value = tiposdedias.FirstOrDefault(e => e.Id == item.Key.CodTdia).DesTdia.Trim();
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder++].Value = bandera.AbrBan.Trim();
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder++].Value = item.Key.TipoHoraDesc.Trim();
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            sheets.Cells[row, cellheder++].Value = item.Key.TotalMin;

                            foreach (var sector in result.Sectores.OrderBy(e => e.Orden))
                            {
                                var min = item.FirstOrDefault()?.HDetaminxtipo?.FirstOrDefault(e => e.CodHsector == sector.CodHsector);
                                sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                sheets.Cells[row, cellheder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                sheets.Cells[row, cellheder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                sheets.Cells[row, cellheder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                sheets.Cells[row, cellheder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                sheets.Cells[row, cellheder].Style.Numberformat.Format = "hh:mm:ss";

                                if (!min.Minuto.HasValue)
                                {
                                    min.Minuto = Convert.ToDecimal(0);
                                }

                                double minutoValue = Convert.ToDouble(Math.Truncate(min.Minuto.Value));
                                double segundoValue = Convert.ToDouble(min.Minuto - Math.Truncate(min.Minuto.Value));

                                TimeSpan hour = TimeSpan.FromMinutes(minutoValue);
                                TimeSpan second = TimeSpan.FromSeconds(segundoValue*100);
                                TimeSpan time = hour.Add(second);

                                sheets.Cells[row, cellheder++].Value = time;

                            }
                        }

                        sheets.Cells.AutoFitColumns();

                    }

                }

                if (excel.Workbook.Worksheets.Count > 0)
                {
                    file.ByteArray = ExcelHelper.GenerateByteArray(excel);
                    file.ForceDownload = true;
                    file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    file.FileName = string.Format("M - {0} - {1} - {2}.xlsx", linea.DesLin.Trim(), bandera.AbrBan.Trim(), fecha.FecDesde.ToString("yyyy.MM.dd"));
                    file.FileDescription = "Exporte Minutos por Sector";
                    files.Add(file);
                }
                
            }

            return files;
        }
    }
}
