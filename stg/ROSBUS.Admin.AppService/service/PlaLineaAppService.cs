using GeoJSON.Net.Geometry;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Constants;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Caching;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;
using static ROSBUS.Admin.AppService.Model.ComoLlegoBusDto;

namespace ROSBUS.Admin.AppService
{

    public class PlaLineaAppService : AppServiceBase<PlaLinea, PlaLineaDto, int, IPlaLineaService>, IPlaLineaAppService
    {
        private ICacheManager _cacheManager;
        private IRamalColorAppService _ramalColorAppService;
        private ISysParametersService _sysParametersService;
        private IBanderaService _banderaService;
        private IHFechasService _hFechasService;
        private ITiposDeDiasService _tiposDeDiasService;
        public PlaLineaAppService(IPlaLineaService serviceBase, ICacheManager cacheManager, IRamalColorAppService ramalColorAppService, ISysParametersService sysParametersService, IBanderaService banderaService, IHFechasService hFechasService, ITiposDeDiasService tiposDeDiasService) 
            :base(serviceBase)
        {
            _cacheManager = cacheManager;
            _ramalColorAppService = ramalColorAppService;
            _sysParametersService = sysParametersService;
            _banderaService = banderaService;
            _hFechasService = hFechasService;
            _tiposDeDiasService = tiposDeDiasService;
        }


        public override Task DeleteAsync(int id)
        {

            if (this._serviceBase.ExistExpression(e=> e.PlaLineaLineaHoraria.Any(p=> p.PlaLineaId==id)))
            {
                throw new ValidationException("No se permite Eliminar la Linea ya que se esta utilizando en una Linea Horario.");
            }

            if (this._serviceBase.ExistExpression(e =>e.Id == id && e.RamalColor.Any(r=> r.HBanderas.Any(b=> b.Rutas.Any(rut=> rut.EstadoRutaId == PlaEstadoRuta.APROBADO)))))
            {
                throw new ValidationException("No se permite Eliminar la Linea ya que tiene rutas aprobadas.");
            }


            return base.DeleteAsync(id);
        }

        public async Task<List<Leg>> GetAllRosBusRoutes(ComoLlegoBusFilter filter)
        {
            RosarioBusRutas Rutas = new RosarioBusRutas();
            List<Leg> RouteDescription = new List<Leg>();
            double TotalACaminar = 0;
            double TotalMetrosRuta = 0;
            int cacheDays = Convert.ToInt32((await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.CLRutasCache)).Items.FirstOrDefault().Value);
            bool primeringreso;
            TotalACaminar = (110 * filter.CantidadCuadras);
            Guid GrupoRutasId = Guid.NewGuid();
            var rutasstepkey = (filter.Origen.Trim() + filter.Destino.Trim() + filter.CantidadCuadras).ToString();
            RouteDescription = await this._cacheManager.GetCache<string, List<Leg>>("RosarioBusStep")
                                 .GetOrDefaultAsync(rutasstepkey);


            if (RouteDescription == null)
            {
                string url = @"https://maps.googleapis.com/maps/api/directions/json?origin=" + filter.Origen + "&destination=" + filter.Destino + "&mode=transit&key=AIzaSyB30elYyYlrIVqshobhWswg6QHGHAeU0Kc&alternatives=true&rankby=distance";
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                var GoogleRoutes = Newtonsoft.Json.JsonConvert.DeserializeObject<RosarioBusRutas>(reader.ReadToEnd());
                response.Close();


                //var OpenFile = new System.IO.StreamReader("C:/Users/Andres/Desktop/GoogleResponse");
                //var GoogleRoutes = Newtonsoft.Json.JsonConvert.DeserializeObject<RosarioBusRutas>(OpenFile.ReadToEnd());



                if (GoogleRoutes.status == "OK")
                {
                    RouteDescription = new List<Leg>();
                    Rutas.geocoded_waypoints = new List<GeocodedWaypoint>(GoogleRoutes.geocoded_waypoints);
                    Rutas.routes = new List<Route>();
                    foreach (var Ruta in GoogleRoutes.routes)
                    {
                        Guid RutaId = Guid.NewGuid();
                        Ruta.RouteId = RutaId;
                        foreach (var leg in Ruta.legs)
                        {
                            primeringreso = true;
                            foreach (var step in leg.steps.Where(e => e.travel_mode.Equals("TRANSIT")))
                            {
                                var ExistRBusRoute = this._serviceBase.ExistExpression(e => e.RamalColor.Any(r => r.RouteLongName.Equals(step.transit_details.line.name) && r.IsDeleted == false));
                                if (ExistRBusRoute && primeringreso)
                                {
                                    foreach (var item in leg.steps.Where(e => e.travel_mode.Equals("WALKING")))
                                    {
                                        TotalMetrosRuta = TotalMetrosRuta + item.distance.value;
                                    }
                                    if (TotalMetrosRuta <= TotalACaminar)
                                    {
                                        primeringreso = false;
                                        leg.ParentRouteId = RutaId;
                                        leg.GrupoRoutesId = GrupoRutasId;
                                        RouteDescription.Add(leg);
                                        Rutas.routes.Add(Ruta);
                                    }

                                }
                            }
                        }
                    }

                    if (RouteDescription.Count > 0)
                    {
                        await this._cacheManager.GetCache<string, List<Leg>>("RosarioBusStep").SetAsync(rutasstepkey, RouteDescription, TimeSpan.FromDays(cacheDays));
                        await this._cacheManager.GetCache<string, RosarioBusRutas>("RosarioBusRutas").SetAsync(GrupoRutasId.ToString(), Rutas, TimeSpan.FromDays(cacheDays));
                        return RouteDescription;
                    }
                    else
                    {
                        throw new ValidationException("NOT_ROUTE_FOUND");
                    }
                }
                else
                {
                    throw new ValidationException("NOT_ROUTE_FOUND");
                }

            }
            else 
            {
                return RouteDescription;
            }
        }

        public async Task<FileDto> GetHorariosRuta(ComoLlegoBusFilter filter)
        {
            string SentidoOrigen = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.CLSentidoOrigen)).Items.FirstOrDefault().Value;
            string SentidoDestino = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.CLSentidoDestino)).Items.FirstOrDefault().Value;
            List<HBanderas> Banderas = new List<HBanderas>();
            HFechas TipoDia = new HFechas();
            string routelongname = "";
            ExcelPackage excel = new ExcelPackage();
            List<string> nomsline = new List<string>();
            var file = new FileDto();

            if (filter.CodBan != null)
            {
                Banderas = (await this._banderaService.GetAllAsync(e => e.RamalColor.PlaLinea.RamalColor.Any(z => z.HBanderas.Any(y => y.Id == filter.CodBan.GetValueOrDefault())))).Items.ToList();
                var bandera = Banderas.Where(e => e.Id == filter.CodBan).FirstOrDefault();
                routelongname = bandera.RamalColor.RouteLongName;
            }
            else
            {
                RosarioBusRutas RouteDetail = new RosarioBusRutas();
                var rosarioBusRutas = await this._cacheManager.GetCache<string, RosarioBusRutas>("RosarioBusRutas")
                                        .GetOrDefaultAsync(filter.GrupoRoutesId.ToString());

                RouteDetail.routes = new List<Route>();
                RouteDetail.routes.Add(rosarioBusRutas.routes.Where(e => e.RouteId == filter.ParentRouteId).FirstOrDefault());

                foreach (var leg in RouteDetail.routes[0].legs)
                {
                    foreach (var step in leg.steps.Where(e => e.travel_mode.Equals("TRANSIT")))
                    {
                        var ExistRBusRoute = this._serviceBase.ExistExpression(e => e.RamalColor.Any(r => r.RouteLongName.Equals(step.transit_details.line.name) && r.IsDeleted == false));
                        if (ExistRBusRoute)
                        {
                            nomsline.Add(step.transit_details.line.name);
                            Banderas.AddRange((await this._banderaService.GetAllAsync(e => e.RamalColor.PlaLinea.RamalColor.Any(z => z.RouteLongName.Equals(step.transit_details.line.name)))).Items.ToList());

                        }

                    }
                }
                routelongname = String.Join(",", nomsline.ToArray());
                //nomsline.Add("103 Negra");
                //nomsline.Add("125");

                foreach (var item in nomsline)
                {
                    Banderas.AddRange((await this._banderaService.GetAllAsync(e => e.RamalColor.PlaLinea.RamalColor.Any(z => z.RouteLongName.Equals(item)))).Items.ToList());
                }
                routelongname = String.Join(",", nomsline.ToArray());
            }

            var Lineas = Banderas.GroupBy(e => e.RamalColor.LineaId);
            var data = await this._serviceBase.GetHorariosRuta(routelongname);

            foreach (var Linea in Lineas)
            {
                var BanderasXLinea = Banderas.Where(e => e.RamalColor.LineaId == Linea.Key);

                var sheets = excel.Workbook.Worksheets.Add(BanderasXLinea.FirstOrDefault().RamalColor.PlaLinea.DesLin.GetValueOrDefault());

                var today = DateTime.Today;
                TipoDia = (await this._hFechasService.GetAllAsync(e => e.Fecha == today && e.Id == BanderasXLinea.FirstOrDefault().RamalColor.LineaId)).Items.FirstOrDefault();
                var DescTipoDia = (await this._tiposDeDiasService.GetByIdAsync(TipoDia.CodTdia));

                var Banderasorigen = BanderasXLinea.Where(e => e.SenBan.Trim().Equals(SentidoOrigen.Trim())).ToList();
                var Banderasdestino = BanderasXLinea.Where(e => e.SenBan.Trim().Equals(SentidoDestino.Trim())).ToList();


                #region DataExcel
                var nombregalponesIda = data.Minutos.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia && Banderasorigen.Any(y => y.Id == e.cod_ban))
                                        .Select(s => new ReporteHorarioPasajerosItemGalpon(s.cod_hsector, s.descripcion_Sector, Convert.ToInt32(s.orden)))
                                        .Distinct().OrderBy(w => w.Orden).ToList();

                var minutosIda = data.Minutos.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia && Banderasorigen.Any(y => y.Id == e.cod_ban))
                                        .Distinct().OrderBy(w => w.orden).ToList();

                var MVIda = data.MediasVueltas.Where(e => e.cod_tdia == TipoDia.CodTdia && Banderasorigen.Any(y => y.Id == e.cod_ban))
                                        .Distinct().OrderBy(w => w.orden).ToList();


                var gruposSectorIdaGalpones = minutosIda.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia).Distinct().OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                var mediasVueltasPorBanderaIda = MVIda.OrderBy(e => e.orden).GroupBy(e => e.cod_ban).ToList();

                List<ReporteHorarioPasajerosItemGalpon> nombreGalponesIdaOrdenados = new List<ReporteHorarioPasajerosItemGalpon>();

                ReporteHorarioPasajerosItemGalpon primerSectorComunIda = new ReporteHorarioPasajerosItemGalpon(0, "", 0);
                foreach (var sector in gruposSectorIdaGalpones)
                {
                    if (sector.ToList().Count == MVIda.Count)
                    {
                        primerSectorComunIda = nombregalponesIda.Where(e => e.key == sector.Key).FirstOrDefault();
                        primerSectorComunIda.OrdenNuevo = 0;
                        nombreGalponesIdaOrdenados.Add(primerSectorComunIda);
                        break;
                    }

                }

                decimal addneworderida = 0.1M;
                foreach (var bandera in Banderasorigen)
                {
                    var MediasVueltasBandera = mediasVueltasPorBanderaIda.Where(e => e.Key == bandera.Id).FirstOrDefault();
                    if (MediasVueltasBandera != null)
                    {
                        foreach (var item in MediasVueltasBandera)
                        {
                            var sectoresMVIda = minutosIda.Where(s => s.codigoMediaVuelta == item.cod_mvuelta && s.MostrarEnReporte == true).OrderBy(s => s.orden);
                            var ordenSectorComunXBanderaIda = sectoresMVIda.Where(e => e.cod_hsector == primerSectorComunIda.key).FirstOrDefault();
                            foreach (var sectorMVIda in sectoresMVIda)
                            {
                                if (nombreGalponesIdaOrdenados.Find(e => e.key == sectorMVIda.cod_hsector) == null)
                                {
                                    if (ordenSectorComunXBanderaIda != null)
                                    {
                                        var sec = nombregalponesIda.Where(s => s.key == sectorMVIda.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden - ordenSectorComunXBanderaIda.orden;
                                        if (nombreGalponesIdaOrdenados.Find(w => w.OrdenNuevo == sec.OrdenNuevo) != null)
                                        {
                                            var secInList = minutosIda.Where(s => s.cod_hsector == sec.key).FirstOrDefault();
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
                var nombregalponesVuelta = data.Minutos.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia)
                    .Select(s => new ReporteHorarioPasajerosItemGalpon(s.cod_hsector, s.descripcion_Sector, Convert.ToInt32(s.orden)))
                    .Distinct().OrderBy(w => w.Orden);

                var minutosVuelta = data.Minutos.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia && Banderasdestino.Any(y => y.Id == e.cod_ban))
                            .Distinct().OrderBy(w => w.orden).ToList();

                var MVVuelta = data.MediasVueltas.Where(e => e.cod_tdia == TipoDia.CodTdia && Banderasdestino.Any(y => y.Id == e.cod_ban))
                                        .Distinct().OrderBy(w => w.orden).ToList();

                var gruposSectorVueltaGalpones = minutosVuelta.Where(e => e.MostrarEnReporte && e.cod_tdia == TipoDia.CodTdia).OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                var mediasVueltasPorBanderaVuelta = MVVuelta.OrderBy(e => e.orden).GroupBy(e => e.cod_ban).ToList();

                List<ReporteHorarioPasajerosItemGalpon> nombreGalponesVueltaOrdenados = new List<ReporteHorarioPasajerosItemGalpon>();

                ReporteHorarioPasajerosItemGalpon primerSectorComunVuelta = new ReporteHorarioPasajerosItemGalpon(0, "", 0);
                foreach (var sector in gruposSectorVueltaGalpones)
                {
                    if (sector.ToList().Count == MVVuelta.Count)
                    {
                        primerSectorComunVuelta = nombregalponesVuelta.Where(e => e.key == sector.Key).FirstOrDefault();
                        primerSectorComunVuelta.OrdenNuevo = 0;
                        nombreGalponesVueltaOrdenados.Add(primerSectorComunVuelta);
                        break;
                    }

                }

                decimal addnewordervuelta = 0.1M;
                foreach (var bandera in Banderasdestino)
                {
                    var MediasVueltasBandera = mediasVueltasPorBanderaVuelta.Where(e => e.Key == bandera.Id).FirstOrDefault();
                    if (MediasVueltasBandera != null)
                    {
                        foreach (var item in MediasVueltasBandera)
                        {
                            var sectoresMVVUelta = minutosVuelta.Where(s => s.codigoMediaVuelta == item.cod_mvuelta && s.MostrarEnReporte == true).OrderBy(s => s.orden);
                            var ordenSectorComunXBanderaVueltas = sectoresMVVUelta.Where(e => e.cod_hsector == primerSectorComunVuelta.key).FirstOrDefault();
                            foreach (var sectorMVVuelta in sectoresMVVUelta)
                            {
                                if (nombreGalponesVueltaOrdenados.Find(e => e.key == sectorMVVuelta.cod_hsector) == null)
                                {
                                    if (ordenSectorComunXBanderaVueltas != null)
                                    {
                                        var sec = nombregalponesVuelta.Where(s => s.key == sectorMVVuelta.cod_hsector).FirstOrDefault();
                                        sec.OrdenNuevo = sec.Orden - ordenSectorComunXBanderaVueltas.orden;
                                        if (nombreGalponesVueltaOrdenados.Find(w => w.OrdenNuevo == sec.OrdenNuevo) != null)
                                        {
                                            var secinlist = minutosVuelta.Where(s => s.cod_hsector == sec.key).FirstOrDefault();
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
                #endregion DataExcel

                int row = 1;
                var cellheder = 1;

                #region Heder

                var primerarow = sheets.Cells[row, cellheder, row, columanstotales];
                primerarow.Style.Font.Size = 20;
                primerarow.Merge = true;
                primerarow.Value = string.Format("Linea {0}", BanderasXLinea.FirstOrDefault().RamalColor.PlaLinea.DesLin.GetValueOrDefault().ToUpper());
                primerarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                primerarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                primerarow.Style.Font.Color.SetColor(Color.Green);
                primerarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                primerarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                row++;

                var segundarow = sheets.Cells[row, cellheder, row, columanstotales];
                segundarow.Style.Font.Size = 20;
                segundarow.Merge = true;
                segundarow.Value = string.Format("Horario {0}", DescTipoDia.DesTdia.GetValueOrDefault().ToUpper());
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
                subtituloIdarow.Value = string.Format("Sentido {0}", SentidoOrigen);
                subtituloIdarow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                subtituloIdarow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                subtituloIdarow.Style.Fill.BackgroundColor.SetColor(Color.White);
                subtituloIdarow.Style.Font.Color.SetColor(Color.Green);
                subtituloIdarow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //row++;

                var subtituloVueltarow = sheets.Cells[row, columnasIda + columnasdinamicasida + espacioColumnas, row, columanstotales];
                subtituloVueltarow.Style.Font.Size = 14;
                subtituloVueltarow.Merge = true;
                subtituloVueltarow.Value = string.Format("Sentido {0}", SentidoDestino);
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
                var grupossectorida = minutosIda.OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                foreach (var sector in grupossectorida)
                {
                    if (sector.ToList().Count == MVIda.Count)
                    {
                        foreach (var item in sector.ToList().OrderBy(e => e.SaleCalculado))
                        {
                            MediasVueltasIdaOrdenadas.Add(item.codigoMediaVuelta);
                        }
                        break;
                    }
                }


                foreach (var mvordenado in MediasVueltasIdaOrdenadas)
                {
                    var mediasvueltas = MVIda.Where(m => m.cod_tdia == TipoDia.CodTdia && m.cod_mvuelta == mvordenado);

                    foreach (var mv in mediasvueltas)
                    {
                        i++;
                        cell = 1;
                        sheets.Cells[row, cell++].Value = mv.Origen;

                        var minutosmv = minutosIda.Where(e => e.codigoMediaVuelta == mv.cod_mvuelta);

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

                //Vuelta
                List<int> MediasVueltasVueltasOrdenadas = new List<int>();
                var grupossectorvuelta = minutosVuelta.OrderBy(e => e.orden).GroupBy(e => e.cod_hsector).ToList();
                foreach (var sector in grupossectorvuelta)
                {
                    if (sector.ToList().Count == MVVuelta.Count)
                    {
                        foreach (var item in sector.ToList().OrderBy(e => e.SaleCalculado))
                        {
                            MediasVueltasVueltasOrdenadas.Add(item.codigoMediaVuelta);
                        }
                        break;
                    }
                }
                row = rowStarBody;
                var lastCell = cell;

                foreach (var mvordenado in MediasVueltasVueltasOrdenadas)
                {
                    var mediasvueltas = MVVuelta.Where(m => m.cod_tdia == TipoDia.CodTdia && m.cod_mvuelta == mvordenado);

                    //Vuelta
                    foreach (var mv in mediasvueltas)
                    {
                        cell = lastCell + 1;

                        sheets.Cells[row, cell++].Value = mv.Origen;

                        var minutosmv = minutosVuelta.Where(e => e.codigoMediaVuelta == mv.cod_mvuelta);
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
            file.FileName = string.Format("Horarios Pasajeros-{0}.xlsx", "Reporte Horario Lineas");
            file.FileDescription = "Reporte Distribucion Coches";

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

        public async Task<FileDto> getParadasRuta(ComoLlegoBusFilter filter)
        {
            List<int> CodBanderas = new List<int>();
            int CodLin = 0;
            List<string> nomsline = new List<string>();
            string Lineas;
            if (filter.GrupoRoutesId != null && filter.ParentRouteId != null)
            {
                RosarioBusRutas RouteDetail = new RosarioBusRutas();
                var rosarioBusRutas = await this._cacheManager.GetCache<string, RosarioBusRutas>("RosarioBusRutas")
                                        .GetOrDefaultAsync(filter.GrupoRoutesId.ToString());

                RouteDetail.routes = new List<Route>();
                RouteDetail.routes.Add(rosarioBusRutas.routes.Where(e => e.RouteId == filter.ParentRouteId).FirstOrDefault());

                foreach (var leg in RouteDetail.routes[0].legs)
                {
                    foreach (var step in leg.steps.Where(e => e.travel_mode.Equals("TRANSIT")))
                    {
                        var ExistRBusRoute = this._serviceBase.ExistExpression(e => e.RamalColor.Any(r => r.RouteLongName.Equals(step.transit_details.line.name) && r.IsDeleted == false));
                        if (ExistRBusRoute)
                        {
                            nomsline.Add(step.transit_details.line.name);

                        }

                    }
                }
            }
            //nomsline.Add("125");
            Lineas = String.Join(",", nomsline.ToArray());
            var result = await this._serviceBase.GetParadasRutas(filter.CodBan, Lineas);

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

            var lineas = result.DetalleParadas.GroupBy(e => e.LineaId);
            foreach (var linea in result.DetalleParadas.GroupBy(e => e.LineaId))
            {
                var sheets = excel.Workbook.Worksheets.Add(linea.FirstOrDefault().Linea.TrimOrNull());
                int lastRow = 0;

                int cellheadertoright = 0;
                
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
                    var primerarow = bandera.FirstOrDefault();
                    int row = 1;
                    var cellheder = 1 + cellheadertoright;

                    setHederFirstCabeceraStyle(sheets.Cells[fromRowFirstCabecera, fromColFirstCabecera + cellheadertoright, toRowFirstCabecera, toColFirstCabecera + cellheadertoright]);
                    //setHederFirstCabeceraStyle(sheets.Cells["A1:D1"]);
                    sheets.Cells[row++, cellheder].Value = string.Format("{0} {1} ({2})", primerarow.Linea.TrimOrNull(), primerarow.Ramal.TrimOrNull(), primerarow.Bandera.TrimOrNull());
                    setHederSecondCabeceraStyle(sheets.Cells[fromRowSecondCabecera, fromColSecondCabecera + cellheadertoright, toRowSecondCabecera, toColSecondCabecera + cellheadertoright]);
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



                    cellheder = 1 + cellheadertoright;

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
                            
                        cellheder = 3 + cellheadertoright;
                        setMergeColumnStyle(sheets.Cells[row, cellheder]);
                        sheets.Cells[row, cellheder++].Value = Parada.Cruce.TrimOrNull();
                        setMergeColumnStyle(sheets.Cells[row, cellheder]);
                        sheets.Cells[row++, cellheder].Value = Parada.CodigoParada.ToString().TrimOrNull();

                        if (!Parada.Calle.Equals(sigcalle) || sigcalle == "")
                        {
                            cellheder = 2 + cellheadertoright;
                            sheets.Cells[rowinicioCalle - cantcrucesxcalle, cellheder].Value = Parada.Calle;

                            range = sheets.Cells[sheets.Cells[rowinicioCalle - cantcrucesxcalle, 2 + cellheadertoright].Address + ":" + sheets.Cells[row - 1, 2 + cellheadertoright].Address];
                            setMergeColumnStyle(range);
                            cantcrucesxcalle = 0;
                        }
                        else 
                        {
                            cantcrucesxcalle++;
                        }

                        if (!Parada.Localidad.Equals(sigloc) || sigloc == "")
                        {
                            cellheder = 1 + cellheadertoright;
                            sheets.Cells[rowinicioLocalidad - cantcrucesxloc, cellheder].Value = Parada.Localidad;

                            range = sheets.Cells[sheets.Cells[rowinicioLocalidad - cantcrucesxloc, 1 + cellheadertoright].Address + ":" + sheets.Cells[row - 1, 1 + cellheadertoright].Address];
                            setMergeColumnStyle(range);

                            cantcrucesxloc = 0;
                        }
                        else
                        {
                            cantcrucesxloc++;
                        }

                        indexParada++;

                    }
                    sheets.Column((1 + cellheadertoright)).Width = 30;
                    sheets.Column((2 + cellheadertoright)).Width = 30;
                    sheets.Column((3 + cellheadertoright)).Width = 30;
                    sheets.Column((4 + cellheadertoright)).Width = 17;


                    lastRow = row;
                    sheets.Cells[lastRow + 2, 1 + cellheadertoright].Value = "Versión: " + primerarow.NombreMapa;
                    cellheadertoright = 6;
                    


                }


                if (excel.Workbook.Worksheets.Count == 0)
                {
                    throw new ValidationException("No se puede generar el reporte porque la bandera no posee paradas cargadas");
                }
            }

            var file = new FileDto();
            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Paradas Ruta.xlsx");
            file.FileDescription = "Reporte Paradas Ruta";

            return file;

        }

        public async Task<RosarioBusRutas> GetRosBusRouteDetail(ComoLlegoBusFilter filter)
        {

            string nlRosBus = "";
            string nlRosBusValidas = "";
            string ciudadpermitida = "";
            string privinciapermitida = "";
            string LocalidadesPermitidas;
            string ciudadOrigenRuta = "";
            string provinciaOrigenRuta =  "";
            string ciudadDestinoRuta = "";
            string provinciaDestinoRuta = "";
            List<string> nomslinevalids = new List<string>();
            List<string> nomsline = new List<string>();
            string tiposdias = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.CLTarifaTipoLinea)).Items.FirstOrDefault().Value;
            LocalidadesPermitidas = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.CLTarifaLocalidad)).Items.FirstOrDefault().Value;
            //string ciudadPermitida = LocalidadesPermitidas.Split(',').GetValue(0).ToString();
            //string provinciaPermitida = LocalidadesPermitidas.Split(',').GetValue(1).ToString();

            RosarioBusRutas RouteDetail = new RosarioBusRutas();
            
            var rosarioBusRutas = await this._cacheManager.GetCache<string, RosarioBusRutas>("RosarioBusRutas")
                                    .GetOrDefaultAsync(filter.GrupoRoutesId.ToString());

            if (rosarioBusRutas != null)
            {
                RouteDetail.geocoded_waypoints = new List<GeocodedWaypoint>(rosarioBusRutas.geocoded_waypoints);
                RouteDetail.routes = new List<Route>();
                RouteDetail.routes.Add(rosarioBusRutas.routes.Where(e => e.RouteId == filter.ParentRouteId).FirstOrDefault());
                foreach (var leg in RouteDetail.routes[0].legs)
                {
                    foreach (var step in leg.steps.Where(e => e.travel_mode.Equals("TRANSIT")))
                    {
                        var ExistRBusRoute = this._serviceBase.ExistExpression(e => e.RamalColor.Any(r => r.RouteLongName.Equals(step.transit_details.line.name) && r.IsDeleted == false));
                        if (ExistRBusRoute)
                        {
                            nomsline.Add(step.transit_details.line.name);

                            if (leg.start_address.Contains(LocalidadesPermitidas) && leg.end_address.Contains(LocalidadesPermitidas))
                            {
                                nomslinevalids.Add(step.transit_details.line.name);
                            }
                        }

                    }
                }
                if (nomsline.Count > 0)
                {
                    nlRosBus = String.Join(",", nomsline.ToArray());
                    var mpago = await this._serviceBase.GetMedioPagosRuta(nlRosBus);
                    RouteDetail.routes[0].MediosPago = mpago;
                }
                if (nomslinevalids.Count > 0)
                {
                    nlRosBusValidas = String.Join(",", nomslinevalids.ToArray());
                    var lineasvalidas = await this._serviceBase.GetLineasValidas(nlRosBusValidas, tiposdias);
                    if (nomslinevalids.Count == lineasvalidas.Count)
                    {
                        var tarifaurbana = await this._serviceBase.GetTarifaUrbana(nomslinevalids[0]);
                        RouteDetail.routes[0].TarifaUrbana = tarifaurbana;
                    }
                }

                return RouteDetail;
            }
            else
            {
                throw new ValidationException("NOT_ROUTE_FOUND");
            }


            
        }

        public bool TieneMapasEnBorrador(int id)
        {
            return this._serviceBase.ExistExpression(e => e.Id==id && e.RamalColor.Any(r => r.HBanderas.Any(b => b.Rutas.Any(rut => rut.EstadoRutaId == PlaEstadoRuta.BORRADOR))));
        }

        public async Task<FileDto> GetTarifasRuta(ComoLlegoBusFilter filter)
        {
            List<int> CodBanderas = new List<int>();
            int CodLin = 0;
            List<string> nomsline = new List<string>();
            string NombresLineas;
            if (filter.GrupoRoutesId != null && filter.ParentRouteId != null)
            {
                RosarioBusRutas RouteDetail = new RosarioBusRutas();
                var rosarioBusRutas = await this._cacheManager.GetCache<string, RosarioBusRutas>("RosarioBusRutas")
                                        .GetOrDefaultAsync(filter.GrupoRoutesId.ToString());

                RouteDetail.routes = new List<Route>();
                RouteDetail.routes.Add(rosarioBusRutas.routes.Where(e => e.RouteId == filter.ParentRouteId).FirstOrDefault());

                foreach (var leg in RouteDetail.routes[0].legs)
                {
                    foreach (var step in leg.steps.Where(e => e.travel_mode.Equals("TRANSIT")))
                    {
                        var ExistRBusRoute = this._serviceBase.ExistExpression(e => e.RamalColor.Any(r => r.RouteLongName.Equals(step.transit_details.line.name) && r.IsDeleted == false));
                        if (ExistRBusRoute)
                        {
                            nomsline.Add(step.transit_details.line.name);

                        }

                    }
                }

            }
            //nomsline.Add("103 Negra");
            //nomsline.Add("125");
            NombresLineas = String.Join(",", nomsline.ToArray());

            var tarifas = await this._serviceBase.GetTarifasRutasAsync(filter.CodBan, NombresLineas);

            var LineasId = tarifas.TarifasPlanasRespaldo.GroupBy(e => e.CodLin);

            ExcelPackage excel = new ExcelPackage();
            var file = new FileDto();

            foreach (var Linea in LineasId)
            {

                var DescLinea = (await this._serviceBase.GetByIdAsync(Linea.Key));
                var TPRxLinea = tarifas.TarifasPlanasRespaldo.Where(e => e.CodLin == Linea.Key);
                var TTRxLinea = tarifas.TarifasTrianguloRespaldo.Where(e => e.CodLin == Linea.Key);

                var sheets = excel.Workbook.Worksheets.Add(DescLinea.DesLin.Trim());

                int row = 1;
                int cellheader = 1;

                if (TPRxLinea.Count() > 0)
                {
                    var firstrow = sheets.Cells[row, cellheader, row, 4];
                    firstrow.Style.Font.Size = 18;
                    firstrow.Merge = true;
                    firstrow.Value = string.Format("Cuadro Tarifario Linea {0}", TPRxLinea.FirstOrDefault().Linea);
                    firstrow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    firstrow.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    firstrow.Style.Font.Color.SetColor(Color.Black);
                    firstrow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    firstrow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    firstrow.Style.Border.BorderAround(ExcelBorderStyle.Medium); 

                    row++;

                    var secondrow = sheets.Cells[row, cellheader, row, 4];
                    secondrow.Style.Font.Size = 16;
                    secondrow.Merge = true;
                    secondrow.Value = string.Format("A partir del {0}", TPRxLinea.FirstOrDefault().FechaActivacion.ToString("dd/MM/yyyy"));
                    secondrow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    secondrow.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    secondrow.Style.Font.Color.SetColor(Color.Black);
                    secondrow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    secondrow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    secondrow.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    row++;

                    //var thirdrow = sheets.Cells[row, cellheader, row, 4];
                    //thirdrow.Style.Font.Size = 14;
                    //thirdrow.Merge = true;
                    //thirdrow.Value = string.Format("Segun Normativa Vigente");
                    //thirdrow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //thirdrow.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    //thirdrow.Style.Font.Color.SetColor(Color.Black);
                    //thirdrow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //thirdrow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //thirdrow.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    //row++;

                    foreach (var item in TPRxLinea)
                    {
                        var DescTar = sheets.Cells[row, cellheader, row, 3];
                        DescTar.Merge = true;
                        DescTar.Value = item.DescripcionTipoBoleto.Trim();
                        DescTar.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        DescTar.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        var PriceTar = sheets.Cells[row, 4];
                        PriceTar.Value = item.Importe;
                        PriceTar.Style.Font.Color.SetColor(Color.Red);
                        PriceTar.Style.Font.Bold = true;
                        PriceTar.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        PriceTar.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        row++;
                    }
                    cellheader = cellheader + 6;
                    sheets.Column(1).Width = 20;
                    sheets.Column(2).Width = 20;
                    sheets.Column(3).Width = 20;
                    sheets.Column(4).Width = 10;
                }

                
                if (TTRxLinea.Count() > 0)
                {
                    row = 1;
                    var ColumnasTotales = (Convert.ToInt32(TTRxLinea.Count()) / 2);

                    var firstrow = sheets.Cells[row, cellheader, row,(cellheader + ColumnasTotales)];
                    firstrow.Style.Font.Size = 18;
                    firstrow.Merge = true;
                    firstrow.Value = string.Format("Cuadro Tarifario Linea {0}", TTRxLinea.FirstOrDefault().Linea);
                    firstrow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    firstrow.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    firstrow.Style.Font.Color.SetColor(Color.Black);
                    firstrow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    firstrow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    firstrow.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    row++;

                    var secondrow = sheets.Cells[row, cellheader, row, (cellheader + ColumnasTotales)];
                    secondrow.Style.Font.Size = 16;
                    secondrow.Merge = true;
                    secondrow.Value = string.Format("A partir del {0}", TTRxLinea.FirstOrDefault().FechaActivacion.ToString("dd/MM/yyyy"));
                    secondrow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    secondrow.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    secondrow.Style.Font.Color.SetColor(Color.Black);
                    secondrow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    secondrow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    secondrow.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    row++;

                    var CiudadesDesde = TTRxLinea.GroupBy(e => e.Desde).Distinct().OrderBy(e => e.Key); 
                    var CiudadesHasta = TTRxLinea.GroupBy(e => e.Hasta).Distinct().OrderBy(e => e.Key);

                    var HastaRow = sheets.Cells[row, cellheader];
                    sheets.Column(cellheader).Width = 15;
                    HastaRow.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    HastaRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    HastaRow.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    foreach (var CiudadHasta in CiudadesHasta)
                    {
                        
                        cellheader++;
                        HastaRow = sheets.Cells[row, cellheader];
                        HastaRow.Value = CiudadHasta.Key;
                        HastaRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        HastaRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        HastaRow.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheets.Column(cellheader).Width = 15;

                    }

                    
                    row++;
                    int moveright = 0;
                    foreach (var CiudadDesde in CiudadesDesde)
                    {
                        cellheader = cellheader - (ColumnasTotales + moveright);                     
                        var DesdeRow = sheets.Cells[row, cellheader++];
                        DesdeRow.Value = CiudadDesde.Key;
                        DesdeRow.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        foreach (var CiudadHasta in CiudadesHasta)
                        {
                            if (CiudadHasta.Key == CiudadDesde.Key)
                            {
                                DesdeRow = sheets.Cells[row, cellheader++];
                                DesdeRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                DesdeRow.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                            else 
                            {
                                DesdeRow = sheets.Cells[row, cellheader++];
                                DesdeRow.Value = CiudadDesde.Where(e => e.Hasta.Equals(CiudadHasta.Key)).FirstOrDefault().Importe;
                                DesdeRow.Style.Font.Bold = true;
                            }
                            DesdeRow.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            DesdeRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        } 
                        
                        row++;
                        moveright = 1;
                    }
                }

            }

            file.ByteArray = ExcelHelper.GenerateByteArray(excel);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Horarios Pasajeros-{0}.xlsx", "Reporte Horario Lineas");
            file.FileDescription = "Reporte Distribucion Coches";

            return file;

        }

        public async Task <RutaWS9> GetRutaBandera(int? codBan)
        {
            var puntosruta = await this._serviceBase.GetPuntosRutaBanderaAsync(codBan);

            RutaWS9 Route = new RutaWS9();
            Route.poswithdata = new List<PositionWithData>();
            Route.Instructions = puntosruta[0].instructions;

            foreach (var item in puntosruta)
            {
                PositionWithData pos = new PositionWithData();
                pos.Latitude = Math.Round(Convert.ToDouble(item.Lat), 6);
                pos.Longitude = Math.Round(Convert.ToDouble(item.Long), 6);
                pos.Data = item.Data;

                Route.poswithdata.Add(pos);

            }

            return Route;
        }

        public DataInfoPuntos getInfoData(PuntosRuta item)
        {
            try
            {
                if (!string.IsNullOrEmpty(item.Data))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DataInfoPuntos>(item.Data);
            }
            catch (Exception)
            {
                return new DataInfoPuntos();
            }

            return new DataInfoPuntos();
        }
    }
}
