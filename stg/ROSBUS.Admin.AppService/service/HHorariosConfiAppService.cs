using OfficeOpenXml;
using OfficeOpenXml.Style;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService
{

    public class HHorariosConfiAppService : AppServiceBase<HHorariosConfi, HHorariosConfiDto, int, IHHorariosConfiService>, IHHorariosConfiAppService
    {
        private readonly IHServiciosService _HServiciosService;
        private readonly IPlaDistribucionDeCochesPorTipoDeDiaService _PlaDistribucionDeCochesPorTipoDeDiaService;
        private readonly IHFechasConfiService _HFechasConfiService;
        private readonly IHChoxserService _HChoxserService;
        private readonly ILineaService _lineaService;

        public IServiceProvider _serviceProvider { get; }

        public HHorariosConfiAppService(IHHorariosConfiService serviceBase,
            IHFechasConfiService HFechasConfiService,
            IHServiciosService HServiciosService,
            IPlaDistribucionDeCochesPorTipoDeDiaService PlaDistribucionDeCochesPorTipoDeDiaService,
            IHChoxserService HChoxserService,
            IServiceProvider serviceProvider,
            ILineaService lineaService
            )
            : base(serviceBase)
        {
            this._HServiciosService = HServiciosService;
            this._PlaDistribucionDeCochesPorTipoDeDiaService = PlaDistribucionDeCochesPorTipoDeDiaService;
            this._HFechasConfiService = HFechasConfiService;
            this._HChoxserService = HChoxserService;
            this._serviceProvider = serviceProvider;
            this._lineaService = lineaService;

        }


        public override async Task<HHorariosConfiDto> AddAsync(HHorariosConfiDto dto)
        {
            HHorariosConfi entity = new HHorariosConfi();

            return await AddOrUpdateInternal(dto, entity);

        }

        public override async Task<HHorariosConfiDto> UpdateAsync(HHorariosConfiDto dto)
        {




            if (!_PlaDistribucionDeCochesPorTipoDeDiaService.ExistExpression(e => e.CodHfecha == dto.CodHfecha && e.CodTdia == dto.CodTdia))
            {
                throw new ValidationException("No existe tipo de dia planificado para el horario");
            }





            var entity = await this.GetByIdAsync(dto.Id);

            return await AddOrUpdateInternal(dto, entity);
        }
        public async Task<HHorariosConfiDto> AddOrUpdateDurYSer(HHorariosConfiDto dto, HChoxserExtendedDto duracion)
        {
            HHorariosConfi entity = new HHorariosConfi();

            return await AddOrUpdateInternal(dto, entity, duracion);
        }

        private async Task<HHorariosConfiDto> AddOrUpdateInternal(HHorariosConfiDto dto, HHorariosConfi entity, HChoxserExtendedDto duracion = null)
        {
            //mapero el galpon 
            MapObject(dto, entity);

            HServicios servicioEntity = new HServicios();
            HServiciosDto servicioDtoOriginal = null;

            if (dto.CurrentServicio.Id > 0)
            {
                servicioEntity = await _HServiciosService.GetByIdAsync(dto.CurrentServicio.Id);
                servicioDtoOriginal = MapObject<HServicios, HServiciosDto>(servicioEntity);
            }



            //mapero el servicio dto a la entidad
            MapObject(dto.CurrentServicio, servicioEntity);

            var MvNuevas = dto.CurrentServicio.HMediasVueltas.Where(e => e.Id < 0);



            var MvModificadas = new List<HMediasVueltasDto>();
            var listademediasvueltasaactualizar = new List<int>();

            var listademediasvueltaseliminar = new List<int>();
            if (servicioDtoOriginal != null)
            {
                var MvEliminadas = servicioDtoOriginal.HMediasVueltas.Where(w => !dto.CurrentServicio.HMediasVueltas.Any(e => e.Id == w.Id));

                foreach (var item in servicioDtoOriginal.HMediasVueltas)
                {
                    var modificado = dto.CurrentServicio.HMediasVueltas.FirstOrDefault(e => e.Id == item.Id);
                    if (modificado != null && HasChange(item, modificado))
                    {
                        MvModificadas.Add(modificado);
                    }
                }

                listademediasvueltaseliminar = MvEliminadas.Select(s => s.Id).Union(MvModificadas.Select(e => e.Id)).ToList();
            }



            foreach (var mv in servicioEntity.HMediasVueltas.Where(e => MvModificadas.Any(a => a.Id == e.Id)))
            {
                mv.DifMin = Convert.ToDecimal((mv.Llega - mv.Sale).TotalMinutes);
            }


            if (dto.CurrentServicio.Id < 0 && duracion == null)
            {
                throw new ValidationException("SERVICIO_CON_DURACION");
            }

            //TODO: revisar el calculo de duracion
            var primerSale = servicioEntity.HMediasVueltas.OrderBy(e => e.Orden).FirstOrDefault()?.Sale;
            var ultimoLlega = servicioEntity.HMediasVueltas.OrderBy(e => e.Orden).LastOrDefault()?.Llega;


            



            if (dto.CurrentServicio.Id > 0 && duracion == null)
            {
                servicioEntity.Duracion = Convert.ToInt32((ultimoLlega.GetValueOrDefault() - primerSale.GetValueOrDefault()).TotalMinutes);
                var primerSaleOriginal = servicioDtoOriginal.HMediasVueltas.OrderBy(e => e.Orden).FirstOrDefault()?.Sale;
                var ultimoLlegaOriginal = servicioDtoOriginal.HMediasVueltas.OrderBy(e => e.Orden).LastOrDefault()?.Llega;
                servicioDtoOriginal.Duracion = Convert.ToInt32((ultimoLlegaOriginal.GetValueOrDefault() - primerSaleOriginal.GetValueOrDefault()).TotalMinutes);
                if (primerSale != primerSaleOriginal || ultimoLlega != ultimoLlegaOriginal || servicioEntity.Duracion != servicioDtoOriginal.Duracion)
                {
                    if (this._HChoxserService.ExistExpression(e => e.Id == dto.CurrentServicio.Id))
                    {
                        throw new ValidationException("SERVICIO_CON_DURACION");
                    }
                }


                if (_HFechasConfiService.ExistExpression(e => e.Id == dto.CodHfecha && e.PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Aprobado))
                {
                    if (await _HFechasConfiService.HorarioDiagramado(dto.CodHfecha, dto.CodTdia, new List<int>() { dto.CurrentServicio.Id }))
                    {
                        throw new ValidationException("El servicio ya fue diagramado");
                    }
                }
            }



            await _HServiciosService.RecrearMinutosPorSector(entity, servicioEntity, listademediasvueltasaactualizar, listademediasvueltaseliminar, duracion);

            var result = MapObject<HHorariosConfi, HHorariosConfiDto>(entity);

            result.CurrentServicio = new HServiciosDto() { Id = servicioEntity.Id };

            return result;
        }

        private static bool HasChange(HMediasVueltasDto w, HMediasVueltasDto e)
        {
            var value = //w.Id == e.Id &&
                            (w.Sale != e.Sale
                            || w.Llega != e.Llega
                            || w.CodBan != e.CodBan
                            || w.CodTpoHora != e.CodTpoHora);


            return value;
        }

        public async Task DeleteDuracionesServicio(int idServicio)
        {
            List<Expression<Func<HServicios, Object>>> includeExpression = new List<Expression<Func<HServicios, object>>>()
            {
                e=> e.CodHconfiNavigation
            };

            var serv = (await this._HServiciosService.GetAllAsync(e => e.Id == idServicio, includeExpression)).Items.FirstOrDefault();

            if (_HFechasConfiService.ExistExpression(e => e.Id == serv.CodHconfiNavigation.CodHfecha && e.PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Aprobado))
            {
                if (await _HFechasConfiService.HorarioDiagramado(serv.CodHconfiNavigation.CodHfecha, serv.CodHconfiNavigation.CodTdia, new List<int>() { idServicio }))
                {
                    throw new ValidationException("El servicio ya fue diagramado");
                }
            }
            await this._HChoxserService.DeleteDuracionesServicio(idServicio);
        }

        public async Task<bool> UpdateCantidadCochesReales(HHorariosConfiDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);

            entity.CantidadCochesReal = dto.CantidadCochesReal;
            entity.CantidadConductoresReal = dto.CantidadConductoresReal;
            await this._serviceBase.UpdateAsync(entity);

            return true;
        }



        public async Task<FileDto> ReporteParadasPasajeros(ReportePasajerosFilter filter)
        {
            var result = await this._serviceBase.ReporteParadasPasajeros(filter);

            ExcelPackage excel = new ExcelPackage();

            Func<ExcelRangeBase, ExcelRangeBase> setHederFirstCabeceraStyle = range =>
            {
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                range.Style.Font.Size = 18;
                range.Merge = true;


                return range;
            };

            Func<ExcelRangeBase, ExcelRangeBase> setHederSecondCabeceraStyle = range =>
            {
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.Font.Size = 16;
                range.Merge = true;


                return range;
            };


            Func<ExcelRangeBase, ExcelRangeBase> setHederLocalidadStyle = range =>
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Bold = true;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(128, 128, 128));
                range.Style.Border.BorderAround(ExcelBorderStyle.Medium);


                return range;
            };

            Func<ExcelRangeBase, ExcelRangeBase> setMergeColumnStyle = range =>
            {
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Merge = true;

                return range;
            };



            foreach (var linea in result.Detalle.GroupBy(e => e.LineaId))
            {
                int lastRow = 0;


                var fromRowFirstCabecera = 1;
                var toRowFirstCabecera = 1;
                var fromColFirstCabecera = 1;
                var toColFirstCabecera = 4;

                var fromRowSecondCabecera = 2;
                var toRowSecondCabecera = 2;
                var fromColSecondCabecera = 1;
                var toColSecondCabecera = 4;

                var banderas = linea.GroupBy(e => e.BanderaId);

                foreach (var bandera in linea.GroupBy(e => e.BanderaId))
                {
                    var sheets = excel.Workbook.Worksheets.Add(bandera.FirstOrDefault()?.DescripcionPasajeros.TrimOrNull());

                    var primerarow = bandera.FirstOrDefault();
                    int row = 1;
                    var cellheder = 1;

                    setHederFirstCabeceraStyle(sheets.Cells[fromRowFirstCabecera, fromColFirstCabecera, toRowFirstCabecera, toColFirstCabecera]);
                    //setHederFirstCabeceraStyle(sheets.Cells["A1:D1"]);
                    sheets.Cells[row++, cellheder].Value = string.Format("{0} {1} ({2})", primerarow.Linea.TrimOrNull(), primerarow.Ramal.TrimOrNull(), primerarow.Bandera.TrimOrNull());
                    setHederSecondCabeceraStyle(sheets.Cells[fromRowSecondCabecera, fromColSecondCabecera, toRowSecondCabecera, toColSecondCabecera]);
                    //setHederSecondCabeceraStyle(sheets.Cells["A2:D2"]);
                    sheets.Cells[row++, cellheder].Value = primerarow.DescripcionPasajeros.TrimOrNull();

                    //row++;

                    setHederLocalidadStyle(sheets.Cells[row, cellheder]);
                    sheets.Cells[row, cellheder++].Value = "LOCALIDAD";
                    setHederLocalidadStyle(sheets.Cells[row, cellheder]);
                    sheets.Cells[row, cellheder++].Value = "CALLE";
                    setHederLocalidadStyle(sheets.Cells[row, cellheder]);
                    sheets.Cells[row, cellheder++].Value = "CRUCE";
                    setHederLocalidadStyle(sheets.Cells[row, cellheder]);
                    sheets.Cells[row++, cellheder++].Value = "CÓDIGO PARADA";



                    cellheder = 1;

                    int cantcrucesxcalle = 0;
                    int cantcrucesxloc = 0;
                    int indexParada = 1;
                    foreach (var Parada in bandera.ToList())
                    {

                        ExcelRange range;

                        int rowinicioLocalidad = row;

                        string sigcalle = "";
                        string sigloc = "";
                        int rowinicioCalle = row;
                        if (indexParada != bandera.Count())
                        {
                            sigcalle = bandera.ToList()[indexParada].Calle;
                            sigloc = bandera.ToList()[indexParada].Localidad;
                        }

                        cellheder = 3;
                        setMergeColumnStyle(sheets.Cells[row, cellheder]);
                        sheets.Cells[row, cellheder++].Value = Parada.Cruce.TrimOrNull();
                        setMergeColumnStyle(sheets.Cells[row, cellheder]);
                        sheets.Cells[row++, cellheder].Value = Parada.CodigoParada.ToString().TrimOrNull();

                        if (!Parada.Calle.Equals(sigcalle) || sigcalle == "")
                        {
                            cellheder = 2;
                            sheets.Cells[rowinicioCalle - cantcrucesxcalle, cellheder].Value = Parada.Calle;

                            range = sheets.Cells[sheets.Cells[rowinicioCalle - cantcrucesxcalle, 2].Address + ":" + sheets.Cells[row - 1, 2].Address];
                            setMergeColumnStyle(range);
                            cantcrucesxcalle = 0;
                        }
                        else
                        {
                            cantcrucesxcalle++;
                        }

                        if (!Parada.Localidad.Equals(sigloc) || sigloc == "")
                        {
                            cellheder = 1;
                            sheets.Cells[rowinicioLocalidad - cantcrucesxloc, cellheder].Value = Parada.Localidad;

                            range = sheets.Cells[sheets.Cells[rowinicioLocalidad - cantcrucesxloc, 1].Address + ":" + sheets.Cells[row - 1, 1].Address];
                            setMergeColumnStyle(range);

                            cantcrucesxloc = 0;
                        }
                        else
                        {
                            cantcrucesxloc++;
                        }

                        indexParada++;

                    }
                    sheets.Column((1)).Width = 30;
                    sheets.Column((2)).Width = 30;
                    sheets.Column((3)).Width = 30;
                    sheets.Column((4)).Width = 17;

                    lastRow = row;

                    sheets.Cells[lastRow + 2, 1].Value = "Versión: " + primerarow.NombreMapa;

                    
                }

            }

         

            var linea2 = await this._lineaService.GetByIdAsync(filter.LineaId);

            //excel.Workbook.Worksheets[0].Cells[lastRow, 0].Value = "Versión: ";

            if (excel.Workbook.Worksheets.Count == 0)
            {
                throw new ValidationException("No se puede generar el reporte porque la bandera no posee paradas cargadas");
            }

            var file = new FileDto();
            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Paradas {0}.xlsx", linea2.DesLin.TrimOrNull());
            file.FileDescription = "Reporte Paradas para Pasajeros";

            return file;
        }

        public async Task<FileDto> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter)
        {
            var r = await this._serviceBase.ReporteDistribucionCoches(filter);


            //var puntos = this._serviceBase.GetAll(filter.GetFilterExpression());
            var file = new FileDto();


            ExcelPackage excel = new ExcelPackage();

            var sheets = excel.Workbook.Worksheets.Add("Coches");

            int row = 1;

            var cellheder = 1;

            Func<ExcelRangeBase, ExcelRangeBase> setHederStyle = range =>
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                range.Style.Font.Bold = true;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(204, 255, 204));
                range.Style.TextRotation = 90;
                range.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                //range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //range.Style.Border.Top.Color.SetColor(Color.Black);
                //range.Style.Border.Bottom.Color.SetColor(Color.Black);
                //range.Style.Border.Left.Color.SetColor(Color.Black);
                //range.Style.Border.Right.Color.SetColor(Color.Black);

                return range;
            };





            var nombregalpones = r.Galpones.Select(s => new { key = s.cod_subg, Name = s.des_subg }).Distinct();
            //whrite heder

            sheets.Cells[row, cellheder].Value = "ID_Linea";

            setHederStyle(sheets.Cells[row, cellheder++]);
            sheets.Cells[row, cellheder].Value = "Linea_dist_coche";
            setHederStyle(sheets.Cells[row, cellheder++]);
            sheets.Cells[row, cellheder].Value = "Tipo de día";
            setHederStyle(sheets.Cells[row, cellheder++]);
            sheets.Cells[row, cellheder].Value = "Fecha";
            setHederStyle(sheets.Cells[row, cellheder++]);
            sheets.Cells[row, cellheder].Value = "Sist Gral";
            setHederStyle(sheets.Cells[row, cellheder++]);

            foreach (var field in nombregalpones)
            {
                sheets.Cells[row, cellheder].Value = field.Name;

                setHederStyle(sheets.Cells[row, cellheder++]);
            }

            sheets.Cells[row, cellheder].Value = "Total Coches";
            setHederStyle(sheets.Cells[row, cellheder]);
            sheets.Cells[row, cellheder].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 0));
            cellheder++;


            sheets.Cells[row, cellheder].Value = "Motivo Cambio";
            setHederStyle(sheets.Cells[row, cellheder]);
            sheets.Cells[row, cellheder].Style.TextRotation = 0;
            sheets.Cells[row, cellheder].Style.Font.Size = 16;
            sheets.Cells[row, cellheder].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 128, 0));
            sheets.Cells[row, cellheder].Style.Font.Color.SetColor(System.Drawing.Color.White);
            cellheder++;

            row++;

            foreach (var item in r.Horarios)
            {

                var cell = 1;
                sheets.Cells[row, cell].Value = item.cod_lin;
                //sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                sheets.Cells[row, cell].Value = item.des_lin.Trim();
                //sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                sheets.Cells[row, cell].Value = item.des_tdia;
                //sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                sheets.Cells[row, cell].Value = item.Fecha.GetValueOrDefault(item.FechaHorario).ToString("dd-MM-yy");
                //sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                sheets.Cells[row, cell].Value = item.FechaHorario.ToString("dd-MM-yy");
                //sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var galponesDelHOrario = r.Galpones.Where(e => e.cod_hfecha == item.cod_hfecha && e.cod_tdia == item.cod_tdia).ToList();

                foreach (var field in nombregalpones)
                {
                    var subgalpon = galponesDelHOrario.FirstOrDefault(e => e.des_subg == field.Name);

                    if (subgalpon != null)
                    {
                        //var name = prop.Name;
                        sheets.Cells[row, cell].Value = subgalpon.CantidadCochesReal;
                    }
                    else
                    {
                        sheets.Cells[row, cell].Value = "";

                    }

                    sheets.Cells[row, cell].Style.Font.Bold = true;
                    sheets.Cells[row, cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    cell++;
                }

                sheets.Cells[row, cell].Value = item.TotalCoches;
                sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 0));
                cell++;

                sheets.Cells[row, cell].Value = item.Motivo;
                sheets.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[row, cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                sheets.Cells[row, cell].Style.Font.Bold = true;
                sheets.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                sheets.Cells[row, cell].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 128, 0));





                cell++;


                row++;
            }

            var rw = 2;
            var rc = 6;
            sheets.View.FreezePanes(rw, rc);





            sheets.Cells.AutoFitColumns();


            sheets.Cells[1, 1, 1, 7 + nombregalpones.Count()].AutoFilter = true;


            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("DistribucionCoches.xlsx", string.Join("-", filter.fecha.GetValueOrDefault().ToString("dd-MM-yy")));
            file.FileDescription = "Reporte Distribucion Coches";

            return file;
        }

        public async Task<FileDto> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter)
        {
            List<DetalleSalidaRelevos> r = await this._serviceBase.ReporteDetalleSalidasYRelevos(filter);

            ITiposDeDiasService tipodisService = (ITiposDeDiasService)_serviceProvider.GetService(typeof(ITiposDeDiasService));
            ILineaService lineaService = (ILineaService)_serviceProvider.GetService(typeof(ILineaService));
            IHFechasService hFechasService = (IHFechasService)_serviceProvider.GetService(typeof(IHFechasService));

            var linea = await lineaService.GetByIdAsync(filter.cod_lin.Value);

            var file = new FileDto();


            var tipodias = (await tipodisService.GetAllAsync(e => true)).Items;

            var TipoDiasGroup = r.GroupBy(t => t.cod_tdia).Distinct();

            var TipoDiasGroupOrdenado = TipoDiasGroup.Select(t => new { tDia = tipodias.FirstOrDefault(e => e.Id == t.Key), items = t.ToList() }).OrderBy(e => e.tDia.Orden);

            ExcelPackage excel = new ExcelPackage();

            int row = 1;

            var cellheder = 1;
            var hfechaConfi = await this._HFechasConfiService.GetByIdAsync(filter.cod_hfecha.Value);
            
            foreach (var dia in TipoDiasGroupOrdenado)
            {
                cellheder = 1;


                var sheets = excel.Workbook.Worksheets.Add(dia.tDia.DesTdia);

                HFechas proximaFecha = await hFechasService.RecuperarProximaFecha(Convert.ToInt32(linea.Id),  dia.tDia.Id, hfechaConfi.FecDesde);


       


                if (!string.IsNullOrEmpty(dia.tDia.Color))
                {
                    sheets.TabColor = System.Drawing.ColorTranslator.FromHtml(dia.tDia.Color);
                }
                #region Heder

                var totalcolumnas = 11;
                sheets.Cells[1, 1, 1, totalcolumnas].Style.Font.Size = 30;
                sheets.Cells[1, 1, 1, totalcolumnas].Merge = true;
                sheets.Cells[1, 1, 1, totalcolumnas].Value = string.Format("Linea {0}", linea.DesLin.GetValueOrDefault().ToUpper());
                sheets.Cells[1, 1, 1, totalcolumnas].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[1, 1, 1, totalcolumnas].Style.Fill.BackgroundColor.SetColor(sheets.TabColor);
                sheets.Cells[1, 1, 1, totalcolumnas].Style.Font.Color.SetColor(sheets.TabColor == Color.Red ? Color.White : Color.Red);
                sheets.Cells[1, 1, 1, totalcolumnas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheets.Cells[1, 1, 1, totalcolumnas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                sheets.Cells[2, 1, 2, totalcolumnas].Style.Font.Size = 20;
                sheets.Cells[2, 1, 2, totalcolumnas].Merge = true;
                sheets.Cells[2, 1, 2, totalcolumnas].Value = string.Format("Horario {0} a partir de {1}", dia.tDia.DesTdia.GetValueOrDefault().ToUpper(), proximaFecha?.Fecha.ToString("dd/MM/yyyy"));
                sheets.Cells[2, 1, 2, totalcolumnas].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheets.Cells[2, 1, 2, totalcolumnas].Style.Fill.BackgroundColor.SetColor(sheets.TabColor);
                sheets.Cells[2, 1, 2, totalcolumnas].Style.Font.Color.SetColor(sheets.TabColor == Color.Red ? Color.White : Color.Red);
                sheets.Cells[2, 1, 2, totalcolumnas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheets.Cells[2, 1, 2, totalcolumnas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                row = 3;

                sheets.Cells[row, cellheder].Value = "Servicios";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Lugar Sale";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Sale";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Llega";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Llega / Sale";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Sale";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Llega";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Llega / Sale";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Sale ";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Hora Llega";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Lugar Llega";
                StyleCellBody(sheets.Cells[row, cellheder++], Bold: true, BackgroundColor: Color.Yellow, ExcelBorder: ExcelBorderStyle.Medium);

                #endregion Heder

                row++;

                foreach (var servicio in dia.items)
                {
                    if (servicio.Sale != null || servicio.SaleAuxiliar != null || servicio.SaleRelevo != null)
                    {

                        String saleLlegaRelevo = servicio.LlegaSaleRelevo.GetValueOrDefault();
                        if (string.IsNullOrEmpty(saleLlegaRelevo) && !servicio.TieneRelevo && !servicio.TieneAuxiliar)
                        {
                            saleLlegaRelevo = servicio.LlegaSaleUltimo.GetValueOrDefault();
                        }

                        string saleLllegaAuxiliar = servicio.LlegaSaleAuxiliar.GetValueOrDefault();
                        if (string.IsNullOrEmpty(saleLllegaAuxiliar) && servicio.TieneRelevo && !servicio.TieneAuxiliar)
                        {
                            saleLllegaAuxiliar = servicio.LlegaSaleUltimo.GetValueOrDefault();
                        }

                        string banderaRelevo = !string.IsNullOrEmpty(servicio.BanderaRelevo.GetValueOrDefault()) ? servicio.BanderaRelevo.GetValueOrDefault() + " - " : null;
                        if (string.IsNullOrEmpty(banderaRelevo) && !servicio.TieneRelevo && !servicio.TieneAuxiliar && !string.IsNullOrEmpty(saleLlegaRelevo))
                        {
                            banderaRelevo = servicio.BanderaUltimo.GetValueOrDefault() + " - ";
                        }
                        string banderaAuxiliar = !string.IsNullOrEmpty(servicio.BanderaAuxiliar.GetValueOrDefault()) ? servicio.BanderaAuxiliar.GetValueOrDefault() + " - " : null;
                        if (string.IsNullOrEmpty(banderaAuxiliar) && servicio.TieneRelevo && !servicio.TieneAuxiliar && !string.IsNullOrEmpty(saleLllegaAuxiliar))
                        {
                            banderaAuxiliar = servicio.BanderaUltimo.GetValueOrDefault() + " - ";
                        }



                        var cell = 1;
                        sheets.Cells[row, cell].Value = servicio.num_ser;
                        StyleCellBody(sheets.Cells[row, cell++], Bold: true, ExcelBorder: ExcelBorderStyle.Medium);


                        //primera
                        sheets.Cells[row, cell].Value = servicio.TienePrimera ? servicio.BanderaSale.GetValueOrDefault() + " - " + servicio.LugarSale.GetValueOrDefault() : "";
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TienePrimera ? Color.Yellow : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);


                        sheets.Cells[row, cell].Value = servicio.Sale.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TienePrimera ? Color.White : Color.Gray, Bold: true, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.Llega.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TienePrimera ? Color.White : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);


                        //relevo
                        sheets.Cells[row, cell].Value = banderaRelevo + saleLlegaRelevo;
                        StyleCellBody(sheets.Cells[row, cell++], !string.IsNullOrEmpty(saleLlegaRelevo.GetValueOrDefault()) ? Color.Yellow : Color.Gray, Bold: true, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.SaleRelevo.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TieneRelevo ? Color.White : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.LlegaRelevo.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TieneRelevo ? Color.White : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);

                        //auxiliar
                        sheets.Cells[row, cell].Value = banderaAuxiliar.GetValueOrDefault() + saleLllegaAuxiliar.GetValueOrDefault();
                        StyleCellBody(sheets.Cells[row, cell++], !string.IsNullOrEmpty(saleLllegaAuxiliar.GetValueOrDefault()) ? Color.Yellow : Color.Gray, Bold: true, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.SaleAuxiliar.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TieneAuxiliar ? Color.White : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.LlegaAuxiliar.ToFormatHoraMinutoString();
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TieneAuxiliar ? Color.White : Color.Gray, ExcelBorder: ExcelBorderStyle.Medium);

                        sheets.Cells[row, cell].Value = servicio.TieneAuxiliar ? servicio.BanderaUltimo.GetValueOrDefault() + " - " + servicio.LlegaSaleUltimo.GetValueOrDefault() : "";
                        StyleCellBody(sheets.Cells[row, cell++], servicio.TieneAuxiliar ? Color.Yellow : Color.Gray, Bold: true, ExcelBorder: ExcelBorderStyle.Medium);

                        row++;
                    }
                    
                }


                sheets.Cells.AutoFitColumns();
            }



            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Detalles Salidas y Relevos - {0}.xlsx", string.Join("-", linea.DesLin));
            file.FileDescription = "Reporte Detalles Salidas y Relevos";

            return file;

        }

        private static void StyleCellBody(
            ExcelRangeBase range,
            Color? BackgroundColor = null,
            Color? Color = null,
            Boolean Bold = false,
            ExcelBorderStyle ExcelBorder = ExcelBorderStyle.None,
            ExcelHorizontalAlignment HorizontalAlignment = ExcelHorizontalAlignment.Center)
        {

            range.Style.Border.BorderAround(ExcelBorder);
            range.Style.Font.Bold = Bold;
            range.Style.HorizontalAlignment = HorizontalAlignment;

            if (Color.HasValue)
            {
                range.Style.Font.Color.SetColor(Color.Value);
            }

            if (BackgroundColor.HasValue)
            {
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(BackgroundColor.Value);
            }



        }

        public async Task<FileDto> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter)
        {
            var data = await this._serviceBase.ReporteHorarioPasajeros(filter);

            ITiposDeDiasService tipodisService = (ITiposDeDiasService)_serviceProvider.GetService(typeof(ITiposDeDiasService));
            ILineaService lineaService = (ILineaService)_serviceProvider.GetService(typeof(ILineaService));
            IHFechasService hFechasService = (IHFechasService)_serviceProvider.GetService(typeof(IHFechasService));

            var linea = await lineaService.GetByIdAsync(filter.cod_lin.Value);
            var file = new FileDto();
            var tipodias = (await tipodisService.GetAllAsync(e => true)).Items;

            ExcelPackage excel = new ExcelPackage();


            var TipoDiasDistinct = data.MediasVueltasIda.Select(t => t.cod_tdia).Union(data.MediasVueltasVueltas.Select(t => t.cod_tdia)).Distinct();
            var TipoDiasOrdenado = tipodias.Where(e => TipoDiasDistinct.Any(a => a == e.Id)).OrderBy(e => e.Orden).ToList();

            var hfechaConfi = await this._HFechasConfiService.GetByIdAsync(filter.codHfecha.Value);
       
            foreach (var dia in TipoDiasOrdenado)
            {
                int row = 1;
                var cellheder = 1;

                HFechas proximaFecha = await hFechasService.RecuperarProximaFecha(Convert.ToInt32(linea.Id), dia.Id, hfechaConfi.FecDesde);

                var  dtd = hfechaConfi.PlaDistribucionDeCochesPorTipoDeDia.FirstOrDefault(e => e.CodTdia == dia.Id);

                var desdehasta = dia.DesTdia.GetValueOrDefault().ToUpper();

                if (dtd != null && !string.IsNullOrEmpty(dtd.Descripcion))
                {
                    desdehasta = dtd.Descripcion.GetValueOrDefault().ToUpper();
                }



                // Orden Columnas Ida
                var nombregalponesIda = data.MinutosIda.Where(e => e.MostrarEnReporte && e.cod_tdia == dia.Id)
                    .Select(s => new ReporteHorarioPasajerosItemGalpon(s.cod_hsector, s.descripcion_Sector, s.orden))
                    .Distinct().OrderBy(w => w.Orden).ToList();


                var gruposSectorIdaGalpones = data.MinutosIda.Where(e => e.MostrarEnReporte && e.cod_tdia == dia.Id).OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                var mediasVueltasPorBanderaIda = data.MediasVueltasIda.OrderBy(e => e.orden).GroupBy(e => e.cod_ban).ToList();

                List<ReporteHorarioPasajerosItemGalpon> nombreGalponesIdaOrdenados = new List<ReporteHorarioPasajerosItemGalpon>();
 
                ReporteHorarioPasajerosItemGalpon primerSectorComunIda = new ReporteHorarioPasajerosItemGalpon(0, "" ,0);
                foreach (var sector in gruposSectorIdaGalpones)
                {
                    if (sector.ToList().Count == data.MediasVueltasIda.Count)
                    {
                        primerSectorComunIda = nombregalponesIda.Where(e => e.key == sector.Key).FirstOrDefault();
                        primerSectorComunIda.OrdenNuevo = 0;
                        nombreGalponesIdaOrdenados.Add(primerSectorComunIda);
                        break;
                    }
                    
                }

                decimal addneworderida = 0.1M;
                foreach (var bandera in filter.BanderasIda)
                {
                    var MediasVueltasBandera = mediasVueltasPorBanderaIda.Where(e => e.Key == bandera).FirstOrDefault();
                    if (MediasVueltasBandera != null)
                    {
                        foreach (var item in MediasVueltasBandera)
                        {
                            var sectoresMVIda = data.MinutosIda.Where(s => s.cod_mvuelta == item.cod_mvuelta && s.MostrarEnReporte == true).OrderBy(s => s.orden);
                            var ordenSectorComunXBanderaIda = sectoresMVIda.Where(e => e.cod_hsector == primerSectorComunIda.key).FirstOrDefault();
                            foreach (var sectorMVIda in sectoresMVIda)
                            {
                                if (nombreGalponesIdaOrdenados.Find(e => e.key == sectorMVIda.cod_hsector) == null)
                                {
                                    if ((ordenSectorComunXBanderaIda != null && sectorMVIda.orden < ordenSectorComunXBanderaIda.orden) || (ordenSectorComunXBanderaIda != null && sectorMVIda.orden > ordenSectorComunXBanderaIda.orden))
                                    {
                                        var sec = nombregalponesIda.Where(s => s.key == sectorMVIda.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden - ordenSectorComunXBanderaIda.orden;
                                        if (nombreGalponesIdaOrdenados.Find(w => w.OrdenNuevo == sec.OrdenNuevo) != null)
                                        {
                                            var secInList = data.MinutosIda.Where(s => s.cod_hsector == sec.key).FirstOrDefault();
                                            if (secInList != null && secInList.SaleCalculado > sectorMVIda.SaleCalculado && sec.OrdenNuevo < 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addneworderida;
                                            }
                                            else if (secInList != null && secInList.SaleCalculado > sectorMVIda.SaleCalculado && sec.OrdenNuevo > 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo - addneworderida;
                                            }
                                            else if (secInList != null && secInList.SaleCalculado < sectorMVIda.SaleCalculado && sec.OrdenNuevo < 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo - addneworderida;

                                            }
                                            else if (secInList != null && secInList.SaleCalculado < sectorMVIda.SaleCalculado && sec.OrdenNuevo > 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addneworderida;

                                            }
                                            else
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addneworderida;
                                            }
                                        }
                                        nombreGalponesIdaOrdenados.Add(sec);
                                        nombreGalponesIdaOrdenados = nombreGalponesIdaOrdenados.OrderBy(e => e.OrdenNuevo).ToList();

                                    }
                                    else if (ordenSectorComunXBanderaIda == null)
                                    {
                                        var sec = nombregalponesIda.Where(s => s.key == sectorMVIda.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden;
                                        nombreGalponesIdaOrdenados.Add(sec);
                                        nombreGalponesIdaOrdenados = nombreGalponesIdaOrdenados.OrderBy(e => e.OrdenNuevo).ToList();
                                    }

                                }

                            }
                            addneworderida += 0.1M;
                            break;
                        }
                    }
                    
                }
                
                
                // Orden Columnas Vueltas
                var nombregalponesVuelta = data.MinutosVueltas.Where(e => e.MostrarEnReporte && e.cod_tdia ==dia.Id)
                    .Select(s => new ReporteHorarioPasajerosItemGalpon(s.cod_hsector, s.descripcion_Sector, s.orden))
                    .Distinct().OrderBy(w => w.Orden);

                var gruposSectorVueltaGalpones = data.MinutosVueltas.Where(e => e.MostrarEnReporte && e.cod_tdia == dia.Id).OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                var mediasVueltasPorBanderaVuelta = data.MediasVueltasVueltas.OrderBy(e => e.orden).GroupBy(e => e.cod_ban).ToList();

                List<ReporteHorarioPasajerosItemGalpon> nombreGalponesVueltaOrdenados = new List<ReporteHorarioPasajerosItemGalpon>();

                ReporteHorarioPasajerosItemGalpon primerSectorComunVuelta = new ReporteHorarioPasajerosItemGalpon(0, "", 0);
                foreach (var sector in gruposSectorVueltaGalpones)
                {
                    if (sector.ToList().Count == data.MediasVueltasVueltas.Count)
                    {
                        primerSectorComunVuelta = nombregalponesVuelta.Where(e => e.key == sector.Key).FirstOrDefault();
                        primerSectorComunVuelta.OrdenNuevo = 0;
                        nombreGalponesVueltaOrdenados.Add(primerSectorComunVuelta);
                        break;
                    }

                }

                decimal addnewordervuelta = 0.1M;
                foreach (var bandera in filter.BanderasVueltas)
                {
                    var MediasVueltasBandera = mediasVueltasPorBanderaVuelta.Where(e => e.Key == bandera).FirstOrDefault();
                    if (MediasVueltasBandera != null)
                    {
                        foreach (var item in MediasVueltasBandera)
                        {
                            var sectoresMVVUelta = data.MinutosVueltas.Where(s => s.cod_mvuelta == item.cod_mvuelta && s.MostrarEnReporte == true).OrderBy(s => s.orden);
                            var ordenSectorComunXBanderaVueltas = sectoresMVVUelta.Where(e => e.cod_hsector == primerSectorComunVuelta.key).FirstOrDefault();
                            foreach (var sectorMVVuelta in sectoresMVVUelta)
                            {
                                if (nombreGalponesVueltaOrdenados.Find(e => e.key == sectorMVVuelta.cod_hsector) == null)
                                {
                                    if ((ordenSectorComunXBanderaVueltas != null && sectorMVVuelta.orden < ordenSectorComunXBanderaVueltas.orden) || (ordenSectorComunXBanderaVueltas != null && sectorMVVuelta.orden > ordenSectorComunXBanderaVueltas.orden))
                                    {
                                        var sec = nombregalponesVuelta.Where(s => s.key == sectorMVVuelta.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden - ordenSectorComunXBanderaVueltas.orden;
                                        if (nombreGalponesVueltaOrdenados.Find(w => w.OrdenNuevo == sec.OrdenNuevo) != null)
                                        {
                                            var secinlist = data.MinutosVueltas.Where(s => s.cod_hsector == sec.key).FirstOrDefault();
                                            if (secinlist != null && secinlist.SaleCalculado > sectorMVVuelta.SaleCalculado && sec.OrdenNuevo < 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addnewordervuelta;
                                            }
                                            else if (secinlist != null && secinlist.SaleCalculado > sectorMVVuelta.SaleCalculado && sec.OrdenNuevo > 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo - addnewordervuelta;
                                            }
                                            else if (secinlist != null && secinlist.SaleCalculado < sectorMVVuelta.SaleCalculado && sec.OrdenNuevo < 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo - addnewordervuelta;

                                            }
                                            else if (secinlist != null && secinlist.SaleCalculado < sectorMVVuelta.SaleCalculado && sec.OrdenNuevo > 0)
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addnewordervuelta;

                                            }
                                            else
                                            {
                                                sec.OrdenNuevo = sec.OrdenNuevo + addnewordervuelta;
                                            }
                                        }
                                        nombreGalponesVueltaOrdenados.Add(sec);
                                        nombreGalponesVueltaOrdenados = nombreGalponesVueltaOrdenados.OrderBy(e => e.OrdenNuevo).ToList();
                                    }
                                    else if (ordenSectorComunXBanderaVueltas == null)
                                    {
                                        var sec = nombregalponesVuelta.Where(s => s.key == sectorMVVuelta.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden;
                                        nombreGalponesVueltaOrdenados.Add(sec);
                                        nombreGalponesVueltaOrdenados = nombreGalponesVueltaOrdenados.OrderBy(e => e.OrdenNuevo).ToList();
                                    }
                                }

                            }
                            addnewordervuelta += 0.1M;
                            break;
                        }
                    }
                    
                }



                //Origen ida  , Destino ida , Bandera ida ,  Espacio , Origen v ,Destino v  ,Bandera v 
                var columnasIda = 3;
                var columnasVuelta = 3;
                var espacioColumnas = 1;
                var columnasfijas = columnasIda + columnasVuelta + espacioColumnas;
                var columanstotales = columnasfijas + nombreGalponesIdaOrdenados.Count() + nombreGalponesVueltaOrdenados.Count();


                var sheets = excel.Workbook.Worksheets.Add(dia.DesTdia);


                if (!string.IsNullOrEmpty(dia.Color))
                {
                    sheets.TabColor = System.Drawing.ColorTranslator.FromHtml(dia.Color);
                }
                #region Heder


                var primerarow = sheets.Cells[row, cellheder, row, columanstotales];
                primerarow.Style.Font.Size = 20;
                primerarow.Merge = true;
                primerarow.Value = string.Format("Linea {0}", linea.DesLin.GetValueOrDefault().ToUpper());
                primerarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                primerarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                primerarow.Style.Font.Color.SetColor(Color.Green);
                primerarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                primerarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                row++;

                var segundarow = sheets.Cells[row, cellheder, row, columanstotales];
                segundarow.Style.Font.Size = 20;
                segundarow.Merge = true;
                segundarow.Value = string.Format("{0} a partir de {1}", desdehasta, proximaFecha?.Fecha.ToString("dd/MM/yyyy"));
                segundarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                segundarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                segundarow.Style.Font.Color.SetColor(Color.Green);
                segundarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                segundarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                row++;

                var columnasdinamicasida = nombreGalponesIdaOrdenados.OrderBy(e => e.OrdenNuevo).Count();
                var subtituloIdarow = sheets.Cells[row, cellheder, row, columnasIda + columnasdinamicasida];
                subtituloIdarow.Style.Font.Size = 14;
                subtituloIdarow.Merge = true;
                subtituloIdarow.Value = string.Format("Sentido {0}", filter.SentidoOrigen);
                subtituloIdarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                subtituloIdarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                subtituloIdarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                subtituloIdarow.Style.Font.Color.SetColor(Color.Green);
                subtituloIdarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //row++;

                var subtituloVueltarow = sheets.Cells[row, columnasIda + columnasdinamicasida + espacioColumnas, row, columanstotales];
                subtituloVueltarow.Style.Font.Size = 14;
                subtituloVueltarow.Merge = true;
                subtituloVueltarow.Value = string.Format("Sentido {0}", filter.SentidoDestino);
                subtituloVueltarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                subtituloVueltarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                subtituloVueltarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                subtituloVueltarow.Style.Font.Color.SetColor(Color.Green);
                subtituloVueltarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                row++;
                sheets.Cells[row, cellheder].Value = "Origen";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);

                foreach (var item in nombreGalponesIdaOrdenados)
                {
                    sheets.Cells[row, cellheder].Value = item.Name;
                    sheets.Cells[row, cellheder].Style.TextRotation = 90;
                    StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);
                }

                sheets.Cells[row, cellheder].Value = "Destino";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Bandera";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);


                //Espacio
                sheets.Cells[row, cellheder++].Value = "  ";


                sheets.Cells[row, cellheder].Value = "Origen";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);

                foreach (var item in nombreGalponesVueltaOrdenados)
                {
                    sheets.Cells[row, cellheder].Value = item.Name;
                    sheets.Cells[row, cellheder].Style.TextRotation = 90;
                    StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);
                }

                sheets.Cells[row, cellheder].Value = "Destino";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);

                sheets.Cells[row, cellheder].Value = "Bandera";
                StyleCellBody(sheets.Cells[row, cellheder++], ExcelBorder: ExcelBorderStyle.Medium);

                #endregion Heder



                row++;
                var rowStarBody = row;
                #region Body
                var cell = 1;
                var i = 0;
                //Ida
                List<int> MediasVueltasIdaOrdenadas = new List<int>();
                var grupossectorida = data.MinutosIda.OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                foreach (var sector in grupossectorida)
                {
                    if (sector.ToList().Count == data.MediasVueltasIda.Count)
                    {
                        foreach (var item in sector.ToList().OrderBy(e => e.SaleCalculado))
                        {
                            MediasVueltasIdaOrdenadas.Add(item.cod_mvuelta);
                        }
                        break;
                    }
                }


                foreach (var mvordenado in MediasVueltasIdaOrdenadas)
                {
                    var mediasvueltas = data.MediasVueltasIda.Where(m => m.cod_tdia == dia.Id && m.cod_mvuelta == mvordenado);

                    foreach (var mv in mediasvueltas)
                    {
                        i++;
                        cell = 1;
                        sheets.Cells[row, cell++].Value = mv.Origen;

                        var minutosmv = data.MinutosIda.Where(e => e.cod_mvuelta == mv.cod_mvuelta);

                        foreach (var item in nombreGalponesIdaOrdenados.OrderBy(e => e.OrdenNuevo).Select(s => s.key).Distinct())
                        {
                            var sector = minutosmv.Where(m => m.cod_hsector == item).FirstOrDefault();
                            if (sector != null && sector.MostrarEnReporte)
                            {
                                sheets.Cells[row, cell].Value = sector.SaleCalculado.ToFormatHoraMinutoString();
                            }
                            StyleCellBody(sheets.Cells[row, cell++], ExcelBorder: ExcelBorderStyle.Medium);
                        }
                        sheets.Cells[row, cell++].Value = mv.Destino;
                        sheets.Cells[row, cell++].Value = mv.PorDonde;

                        row++;
                    }
                }

                List<int> MediasVueltasVueltasOrdenadas = new List<int>();
                var grupossectorvuelta = data.MinutosVueltas.OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                foreach (var sector in grupossectorvuelta)
                {
                    if (sector.ToList().Count == data.MediasVueltasVueltas.Count)
                    {
                        foreach (var item in sector.ToList().OrderBy(e => e.SaleCalculado))
                        {
                            MediasVueltasVueltasOrdenadas.Add(item.cod_mvuelta);
                        }
                        break;
                    }
                }
                row = rowStarBody;
                var lastCell = cell;

                foreach (var mvordenado in MediasVueltasVueltasOrdenadas)
                {
                    var mediasvueltas = data.MediasVueltasVueltas.Where(m => m.cod_tdia == dia.Id && m.cod_mvuelta == mvordenado);

                    //Vuelta
                    foreach (var mv in mediasvueltas)
                    {
                        cell = lastCell + 1;

                        sheets.Cells[row, cell++].Value = mv.Origen;

                        var minutosmv = data.MinutosVueltas.Where(e => e.cod_mvuelta == mv.cod_mvuelta);
                        foreach (var item in nombreGalponesVueltaOrdenados.OrderBy(e => e.OrdenNuevo).Select(s => s.key).Distinct())
                        {
                            var sector = minutosmv.Where(m => m.cod_hsector == item).FirstOrDefault();
                            if (sector != null && sector.MostrarEnReporte)
                            {
                                sheets.Cells[row, cell].Value = sector.SaleCalculado.ToFormatHoraMinutoString();
                            }
                            StyleCellBody(sheets.Cells[row, cell++], ExcelBorder: ExcelBorderStyle.Medium);
                        }
                        sheets.Cells[row, cell++].Value = mv.Destino;
                        sheets.Cells[row, cell++].Value = mv.PorDonde;

                        row++;

                    }
                }

                #endregion

                sheets.Cells.AutoFitColumns();
            }

            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Horarios Pasajeros-{0}.xlsx", string.Join("-", linea.DesLin));
            file.FileDescription = "Reporte Distribucion Coches";

            return file;
        }

    }

    public static class Formaters
    {

        public static string GetValueOrDefault(this string valor)
        {
            if (!String.IsNullOrEmpty(valor))
            {
                return valor.Trim();
            }
            return "";
        }

        public static string ToFormatHoraMinutoString(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("HH:mm");
            }
            return "";
        }


    }
}
