using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class BanderaAppService : AppServiceBase<HBanderas, BanderaDto, int, IBanderaService>, IBanderaAppService
    {
        protected readonly IRutasService _rutaservice;
        protected readonly IHServiciosAppService _hservicios;
        private readonly IPlaDistribucionDeCochesPorTipoDeDiaAppService plaDistribucionDeCochesPorTipoDeDiaAppService;

        public BanderaAppService(IBanderaService serviceBase, IRutasService rutaservice, IHServiciosAppService hservicios, IPlaDistribucionDeCochesPorTipoDeDiaAppService plaDistribucionDeCochesPorTipoDeDiaAppService)
            : base(serviceBase)
        {
            this._rutaservice = rutaservice;
            this._hservicios = hservicios;
            this.plaDistribucionDeCochesPorTipoDeDiaAppService = plaDistribucionDeCochesPorTipoDeDiaAppService;
        }

        public async Task<List<string>> OrigenPredictivo(BanderaFilter filtro)
        {

            return await this._serviceBase.OrigenPredictivo(filtro);
        }

        public async Task<List<string>> DestinoPredictivo(BanderaFilter filtro)
        {

            return await this._serviceBase.DestinoPredictivo(filtro);
        }



        public async Task<FileDto> GetReporteSabanaSinMinutos(HorariosPorSectorDto horarios)
        {

            var file = new FileDto();

            ExcelPackage excel = new ExcelPackage();
            var sheets = excel.Workbook.Worksheets.Add("Reporte");
            int row = 1;
            var cellheder = 1;

            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Servicio";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Sale";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            foreach (var item in horarios.Colulmnas)
            {
                sheets.Cells[row, cellheder++].Value = item.Label;
                sheets.Cells[row, cellheder].Style.Font.Bold = true;
            }
            sheets.Cells[row, cellheder++].Value = "Llega";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "TotalDeMinutos";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "TipoHora";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Bandera";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Frecuencia";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;

            foreach (var item in horarios.Items)
            {
                row++;
                cellheder = 1;
                sheets.Cells[row, cellheder++].Value = item.Servicio;
                sheets.Cells[row, cellheder++].Value = item.Sale;

                string Llega = item.SaleValue.TimeOfDay.Add(TimeSpan.FromMinutes(Convert.ToDouble(item.TotalDeMinutos))).ToString("hh\\:mm\\:ss");
                foreach (var dinamic in item.ColumnasDinamicas)
                {



                    if (dinamic.Hora != null && dinamic.Hora != item.SaleValue.TimeOfDay && !dinamic.HoraFormated.Equals(Llega))
                    {
                        sheets.Cells[row, cellheder++].Value = dinamic.HoraFormated;
                    }
                    else
                    {
                        sheets.Cells[row, cellheder++].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                    }
                   
                }

                sheets.Cells[row, cellheder++].Value = item.Llega;
                sheets.Cells[row, cellheder++].Value = item.TotalDeMinutos;
                sheets.Cells[row, cellheder++].Value = item.DesTipoHora;
                sheets.Cells[row, cellheder++].Value = item.Bandera;
                sheets.Cells[row, cellheder++].Value = item.Diferencia;
            }

            sheets.Cells.AutoFitColumns();

            
            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("S - {0} – {1} - {2} – {3} (Sin minutos).xlsx", horarios.Linea, horarios.LabelBandera, horarios.TipoDia, horarios.FechaDesde.ToString("yyyy.MM.dd"));
            file.FileDescription = "Reporte de Sabana sin minutos entre sectores";

            return file;
        }

        public async Task<FileDto> GetReporteSabanaConMinutos(HorariosPorSectorDto horarios)
        {

            var file = new FileDto();

            ExcelPackage excel = new ExcelPackage();
            var sheets = excel.Workbook.Worksheets.Add("Reporte");
            int row = 1;
            var cellheder = 1;

            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Servicio";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Sale";
            sheets.Cells[row, cellheder++].Value = "Min entre Sector";
            foreach (var item in horarios.Colulmnas)
            {
                sheets.Cells[row, cellheder].Style.Font.Bold = true;
                sheets.Cells[row, cellheder++].Value = item.Label;
                if (Convert.ToInt32(item.Key) <= horarios.Colulmnas.Count)
                {
                    sheets.Cells[row, cellheder++].Value = "Min entre Sector";
                }
                
            }
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Llega";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "TotalDeMinutos";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "TipoHora";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Bandera";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;
            sheets.Cells[row, cellheder++].Value = "Frecuencia";
            sheets.Cells[row, cellheder].Style.Font.Bold = true;

            foreach (var item in horarios.Items)
            {
                row++;
                cellheder = 1;
                int nroServ = 0;
                int.TryParse(item.Servicio.TrimStart('0'), out nroServ);
                sheets.Cells[row, cellheder++].Value = nroServ;
                sheets.Cells[row, cellheder++].Value = item.Sale;
                var primersector = item.ColumnasDinamicas.OrderBy(e => e.Orden).Where(e => e.Hora != null && e.Hora != item.SaleValue.TimeOfDay).FirstOrDefault();
                sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheets.Cells[row, cellheder].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cellheder].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);

                var cell = sheets.Cells[row, cellheder++];
                cell.Value = (primersector.Hora - item.SaleValue.TimeOfDay).Value;//.ToString("hh\\:mm\\:ss");
                cell.Style.Numberformat.Format = "h:mm:ss";


                foreach (var dinamic in item.ColumnasDinamicas)
                {
                    TimeSpan Llega = item.SaleValue.TimeOfDay.Add(TimeSpan.FromMinutes(Convert.ToDouble(item.TotalDeMinutos)));
                    string LlegaFormated = Llega.ToString("hh\\:mm\\:ss");
                    if (dinamic.Hora !=null && dinamic.Hora != item.SaleValue.TimeOfDay && !dinamic.HoraFormated.Equals(LlegaFormated))
                    {
                        sheets.Cells[row, cellheder++].Value = dinamic.HoraFormated;
                    }
                    else
                    {
                        sheets.Cells[row, cellheder++].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                    }
                    
                    var sectorsiguiente = item.ColumnasDinamicas.Where(e => e.Orden == (dinamic.Orden + 1)).FirstOrDefault();
                    if (sectorsiguiente != null)
                    {
                        if (sectorsiguiente.Hora != null && sectorsiguiente.Hora != item.SaleValue.TimeOfDay && dinamic.Hora != null && dinamic.Hora != item.SaleValue.TimeOfDay)
                        {
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            sheets.Cells[row, cellheder].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                            if (dinamic.Hora.Value.Days > sectorsiguiente.Hora.Value.Days)
                            {
                                TimeSpan siguiente = sectorsiguiente.Hora.Value.Add(TimeSpan.FromHours(24));
                                cell = sheets.Cells[row, cellheder++];
                                cell.Value = (siguiente - dinamic.Hora).Value;
                                cell.Style.Numberformat.Format = "h:mm:ss";
                            }
                            else
                            {
                                cell = sheets.Cells[row, cellheder++];
                                cell.Value = (sectorsiguiente.Hora - dinamic.Hora).Value;
                                cell.Style.Numberformat.Format = "h:mm:ss";
                            }

                        }
                        else
                        {
                            sheets.Cells[row, cellheder++].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                        }
                        
                        

                    }
                    else
                    {
                        
                        if (dinamic.Hora != null)
                        {
                            
                            sheets.Cells[row, cellheder].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheets.Cells[row, cellheder].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            sheets.Cells[row, cellheder].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                            cell = sheets.Cells[row, cellheder++];
                            cell.Value = (Llega - dinamic.Hora).Value;
                            cell.Style.Numberformat.Format = "h:mm:ss";
                        }
                        else
                        {
                            sheets.Cells[row, cellheder++].Value = null;
                        }
                    }
                    
                }

                sheets.Cells[row, cellheder++].Value = item.Llega;
                sheets.Cells[row, cellheder++].Value = item.TotalDeMinutos;
                sheets.Cells[row, cellheder++].Value = item.DesTipoHora;
                sheets.Cells[row, cellheder++].Value = item.Bandera;
                sheets.Cells[row, cellheder++].Value = item.Diferencia;
            }

            sheets.Cells.AutoFitColumns();


            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("S - {0} – {1} - {2} – {3} (Con minutos).xlsx", horarios.Linea, horarios.LabelBandera, horarios.TipoDia, horarios.FechaDesde.ToString("yyyy.MM.dd"));
            file.FileDescription = "Reporte de Sabana con minutos entre sectores";

            return file;
        }

        public async Task<FileDto> GetReporteCambiosDeSector(BanderaFilter filter)
        {
            var file = new FileDto();
            //var puntos = this._serviceBase.GetAll(filter.GetFilterExpression());
            List<ReporteCambiosPorSector> items = await this._serviceBase.GetReporteCambiosDeSector(filter);
            ExcelParameters<ReporteCambiosPorSector> parametros = new ExcelParameters<ReporteCambiosPorSector>();

            parametros.HeaderText = null;
            parametros.SheetName = string.Format("Bandera {0}", filter.cod_band);
            parametros.Values = items;
            parametros.AddField("NumeroExterno", "Num. Externo");
            parametros.AddField("Abreviacion", "Abreviacion");
            parametros.AddField("Long", "Coordenada X");
            parametros.AddField("Lat", "Coordenada Y");
            parametros.AddField("Descripcion", "Descripcion");

            file.ByteArray = ExcelHelper.GenerateByteArray(parametros);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("CambiosDeSector-{0}.xlsx", filter.cod_band);
            file.FileDescription = "Reporte de Cambios de Sector";

            return file;
        }

        public async Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter filtro)
        {
            
            List<ItemDto<int>> list = await this._serviceBase.RecuperarBanderasRelacionadasPorSector(filtro);
            
            return list;

        }

        public Task<String> RecuperarCartel(int idBandera)
        {
            return this._serviceBase.RecuperarCartel(idBandera);
        }


        public async Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro)
        {
            if (filtro.ValidarMediasVueltasIncompletas)
            {
                var filter = new PlaDistribucionDeCochesPorTipoDeDia();

                filter.CodHfecha = filtro.CodHfecha ?? throw new ValidationException("El codigo de H_Fecha es Requerido.");
                filter.CodTdia = filtro.CodTdia ?? throw new ValidationException("El codigo de Tipo de Dia es Requerido."); ;
                filter.Banderas = filtro.BanderasSeleccionadas;
                

                var data = await this.plaDistribucionDeCochesPorTipoDeDiaAppService.ExistenMediasVueltasIncompletas(filter);

                if (data.Estado != PlaDistribucionEstadoEnum.Valido)
                {
                    throw new ValidationException("Existen Medias vueltas incompletas, por favor cargue los minutos de todas las mediavueltas.");
                }

            }
            return await this._serviceBase.RecuperarHorariosSectorPorSector(filtro); 
        }

        public async Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter filtro)
        {
            return await this._serviceBase.RecuperarLineasActivasPorFecha(filtro);

        }

        public async Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter filtro)
        {
            return await this._serviceBase.RecuperarBanderasPorServicio(filtro);
        }

        public override async Task<BanderaDto> AddAsync(BanderaDto dto)
        {

            var entity = MapObject<BanderaDto, HBanderas>(dto);

            foreach (var item in entity.PlaCodigoSubeBandera.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            } 

            return MapObject<HBanderas, BanderaDto>(await this.AddAsync(entity));

           
        
        }

        public override Task DeleteAsync(int id)
        {
            if (this._serviceBase.ExistExpression(e => e.Id == id  && e.Rutas.Any(rut => rut.EstadoRutaId == PlaEstadoRuta.APROBADO)))
            {
                throw new ValidationException("No se permite Eliminar la Bandera ya que tiene rutas aprobadas.");
            }

            return base.DeleteAsync(id);
        }

        public async override Task<BanderaDto> UpdateAsync(BanderaDto dto)
        {

            if (dto.TipoBanderaId == PlaTipoBandera.Comerciales)
            {
                var entitydes = await this._serviceBase.GetAllAsync(e => e.DescripcionPasajeros == dto.DescripcionPasajeros && e.Id != dto.Id);
                if (entitydes.TotalCount > 0)
                {
                    throw new ValidationException("No se puede repetir la descripcion para pasajeros para dos banderas distintas");
                }
            }
            
            dto.Rutas.Clear();

            var entity = await this.GetByIdAsync(dto.Id);

            if (dto.Activo)
            {
                if (!entity.Activo.GetValueOrDefault())
                {
                    var filtro = new RutasFilter
                    {
                        EstadoRutaId = PlaEstadoRuta.APROBADO
                    };

                    var rutasaprobadas = await this._rutaservice.GetAllAsync(filtro.GetFilterExpression());

                    if (!rutasaprobadas.Items.Any())
                    {
                        throw new DomainValidationException("No existen bandera aprobadas");
                    }
                }
            }



            MapObject(dto, entity);


            foreach (var item in entity.PlaCodigoSubeBandera.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }


            await this.UpdateAsync(entity);
            return MapObject<HBanderas, BanderaDto>(entity);

        }

        public async Task<List<BanderasLineasDto>> GetAllBanderasLineaAsync(ComoLlegoBusFilter filter)
        {
            List<BanderasLineasDto> BanderasLinea = new List<BanderasLineasDto>();


            BanderaFilter banderaFilter = new BanderaFilter();
            banderaFilter.LineaId = (decimal)filter.CodLin;
            var Banderas = (await this._serviceBase.GetAllAsync(banderaFilter.GetFilterExpression())).Items.ToList();

            foreach (var Bandera in Banderas)
            {
                BanderasLineasDto BanderaLinea = new BanderasLineasDto();
                BanderaLinea.CodBandera = Bandera.Id;
                BanderaLinea.descripcionPasajeros = Bandera.DescripcionPasajeros;
                BanderasLinea.Add(BanderaLinea);
            }

            return BanderasLinea;
        }
    }
}
