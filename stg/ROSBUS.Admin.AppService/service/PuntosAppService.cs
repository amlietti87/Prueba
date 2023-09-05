using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.DocumentsHelper.Excel;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using TECSO.FWK.Domain.Extensions;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Url;
using TECSO.FWK.Extensions;
using SharpKml.Base;
using SharpKml.Dom;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using SharpKml.Engine;
using System.IO;
using ROSBUS.Admin.Domain.Maps;


namespace ROSBUS.Admin.AppService
{

    public class PuntosAppService : AppServiceBase<PlaPuntos, PuntosDto, Guid, IPuntosService>, IPuntosAppService
    {
        private readonly ISectorAppService sectorAppService;
        private readonly IAppUrlService appUrlService;
        private readonly ICoordenadasService _coordenadasService;
        private readonly IRutasService _rutaService;
        private readonly ISharpKMLHelper _sharpKML;
        IPlaSentidoBanderaService _plaSentidoBanderaService;

        public PuntosAppService(IPuntosService serviceBase, ISharpKMLHelper sharpKML, IRutasService rutaService, ICoordenadasService coordenadasService, ISectorAppService _sectorAppService, IAppUrlService _appUrlService)
            : base(serviceBase)
        {
            sectorAppService = _sectorAppService;
            appUrlService = _appUrlService;
            _coordenadasService = coordenadasService;
            _rutaService = rutaService;
            _sharpKML = sharpKML;
            _appUrlService = appUrlService;
        }

        public async Task<List<PuntosDto>> RecuperarDatosIniciales(int CodRec)
        {
            var data = await this._serviceBase.RecuperarDatosIniciales(CodRec);
            var result = new List<PuntosDto>();

            int orden = 0;

            if (data.Any())
            {
                var max = data.Max(e => e.Cuenta);
                var min = data.Min(e => e.Cuenta);


                foreach (var item in data)
                {
                    result.Add(new PuntosDto()
                    {
                        Id = Guid.NewGuid(),
                        Lat = item.Lat * -1,
                        Long = item.Lon * -1,
                        //, CodigoNombre
                        //, Descripcion 
                        //, Data 
                        RutaId = CodRec,
                        EsPuntoInicio = item.Cuenta == min,
                        EsPuntoTermino = item.Cuenta == max,
                        EsParada = false,
                        EsCambioSector = !(item.Cuenta == min || item.Cuenta == max),
                        Orden = orden++
                        //, TipoParadaId =
                        //, NumeroExterno 
                        //, Abreviacion 
                        //, Color   
                    });
                }
            }




            return result;
        }

        public async Task<FileDto> GetReporte(PuntosFilter filter)
        {
            var file = new FileDto();
            //var puntos = this._serviceBase.GetAll(filter.GetFilterExpression());
            var puntos = await this._serviceBase.GetAllAsync(filter.GetPuntosReporteExpression());
            ExcelParameters<PlaPuntos> parametros = new ExcelParameters<PlaPuntos>();

            parametros.HeaderText = null;
            parametros.SheetName = string.Format("Ruta {0}", filter.RutaId);
            parametros.Values = puntos.Items.OrderBy(e => e.Orden).ToList();
            parametros.AddField("NumeroExterno", "Num. Externo");
            parametros.AddField("Long", "Coordenada X");
            parametros.AddField("Lat", "Coordenada Y");
            parametros.AddField("DescripcionCoordenada", "Descripcion");
            parametros.AddField("Orden", "Orden");

            file.ByteArray = ExcelHelper.GenerateByteArray(parametros);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Puntos-Ruta-{0}.xlsx", filter.RutaId);
            file.FileDescription = "Archivo de prueba";

            return file;
        }

        public async Task<List<PuntosDto>> RecuperarPuntosRecorrido(int CodRec)
        {
            var puntos = await this.GetAllAsync(e => e.CodRec == CodRec, new List<Expression<Func<PlaPuntos, object>>>() { s => s.SectorPuntoFin, s=>s.Ruta, s=>s.Ruta.Bandera.SentidoBandera });

            foreach (var punto in puntos.Items)
            {
                punto.Color = "#" + punto.Ruta.Bandera.SentidoBandera.Color;
            }

            return this.MapList<PlaPuntos, PuntosDto>(puntos.Items).ToList();
        }


        public async Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec)
        {
            var data = await this._serviceBase.RecuperarDetaReco(CodRec);

            return data;

        }


        public async Task<FeatureCollection> RecuperarDetaRecoGeoJson(int CodRec)
        {
            var puntos = await this.RecuperarDetaReco(CodRec);


            GeoJSON.Net.Feature.FeatureCollection featureCollection = new GeoJSON.Net.Feature.FeatureCollection();
            List<GeoJSON.Net.Geometry.Point> pointsList = new List<GeoJSON.Net.Geometry.Point>();
            foreach (var item in puntos)
            {
                var position = new Position(Convert.ToDouble(item.Lat * -1), Convert.ToDouble(item.Lon * -1));
                pointsList.Add(new GeoJSON.Net.Geometry.Point(position));
            }
            MultiPoint multiPoint = new MultiPoint(pointsList);

            var mp = new GeoJSON.Net.Feature.Feature(multiPoint);



            featureCollection.Features.Add(mp);

            return featureCollection;
        }

        public async Task<FeatureCollection> RecuperarRecorridoGeoJson(int CodRec)
        {

            var puntos = await this.RecuperarPuntosRecorrido(CodRec);

            SectorFilter filter = new SectorFilter();
            filter.RutaId = CodRec;
            var sectores = await this.sectorAppService.GetAllAsync(filter.GetFilterExpression());

            puntos = puntos.OrderBy(e => e.Orden).ToList();

            string defaultColor = "#000000";

            foreach (var item in sectores.Items)
            {
                foreach (var putoSector in this.getPuntosBySector(item, puntos))
                {
                    if (putoSector.Color.IsNullOrEmpty())
                    {
                        putoSector.Color = item.Color ?? defaultColor;
                    }
                }
            }


            puntos[0].Color = puntos[1].Color;

            GeoJSON.Net.Feature.FeatureCollection featureCollection = new GeoJSON.Net.Feature.FeatureCollection();


            featureCollection.Features.AddRange(this.BuildTramos(puntos, CodRec));

            featureCollection.Features.Add(this.BuildParadas(puntos, CodRec));
            featureCollection.Features.Add(this.BuildPuntoFin(puntos, CodRec));
            featureCollection.Features.Add(this.BuildPuntoInicio(puntos, CodRec));
            featureCollection.Features.Add(this.BuildSectores(puntos, CodRec));
            return featureCollection;
        }




        public async Task<FileDto> CreateKML(PuntosFilter filtro)
        {

            if (!filtro.CodRec.HasValue)
            {
                throw new ValidationException("Falta codigo de linea");
            }

            var ruta = (await this._rutaService.GetAllAsync(e => e.Id == filtro.CodRec, new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Bandera,
                    e=> e.Bandera.SentidoBandera,
                    e=> e.Bandera.RamalColor,
                   e=> e.Bandera.RamalColor.PlaLinea,
                    e=> e.EstadoRuta
                })).Items.FirstOrDefault();

            Kml kml = new Kml();

            kml.AddNamespacePrefix(KmlNamespaces.GX22Prefix, KmlNamespaces.GX22Namespace);

            Document document = new Document();

            Style normalStyle = this._sharpKML.CreatePlacemarkLineStyle("linestring", false, ruta.Bandera.SentidoBandera.Color);
            document.AddStyle(normalStyle);
            Style highlightStyle = this._sharpKML.CreatePlacemarkLineStyle("linestring", true, ruta.Bandera.SentidoBandera.Color);
            document.AddStyle(highlightStyle);
            var linestringMap = this._sharpKML.CreatePlacemarkStyleMap("linestring", normalStyle, highlightStyle);
            document.AddStyle(linestringMap);

            Style StartNormal = this._sharpKML.CreatePlacemarkStartFinish("Start", false, "https://maps.google.com/mapfiles/kml/paddle/I.png");
            document.AddStyle(StartNormal);
            Style StartHigh = this._sharpKML.CreatePlacemarkStartFinish("Start", true, "https://maps.google.com/mapfiles/kml/paddle/I.png");
            document.AddStyle(StartHigh);
            var StartMap = this._sharpKML.CreatePlacemarkStyleMap("Start", StartNormal, StartHigh);
            document.AddStyle(StartMap);

            Style FinishNormal = this._sharpKML.CreatePlacemarkStartFinish("Finish", false, "https://maps.google.com/mapfiles/kml/paddle/F.png");
            document.AddStyle(FinishNormal);
            Style FinishHigh = this._sharpKML.CreatePlacemarkStartFinish("Finish", true, "https://maps.google.com/mapfiles/kml/paddle/F.png");
            document.AddStyle(FinishHigh);
            var FinishMap = this._sharpKML.CreatePlacemarkStyleMap("Finish", FinishNormal, FinishHigh);
            document.AddStyle(FinishMap);

            Style PincheSectorNormal = this._sharpKML.CreatePlacemarkPinche("PincheSector", false, "https://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
            document.AddStyle(PincheSectorNormal);
            Style PincheSectorHigh = this._sharpKML.CreatePlacemarkPinche("PincheSector", true, "https://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
            document.AddStyle(PincheSectorHigh);
            var PincheSectorMap = this._sharpKML.CreatePlacemarkStyleMap("PincheSector", PincheSectorNormal, PincheSectorHigh);
            document.AddStyle(PincheSectorMap);

            Style PincheParadaNormal = this._sharpKML.CreatePlacemarkPinche("PincheParada", false, "https://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png");
            document.AddStyle(PincheParadaNormal);
            Style PincheParadaHigh = this._sharpKML.CreatePlacemarkPinche("PincheParada", true, "https://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png");
            document.AddStyle(PincheParadaHigh);
            var PincheParadaMap = this._sharpKML.CreatePlacemarkStyleMap("PincheParada", PincheParadaNormal, PincheParadaHigh);
            document.AddStyle(PincheParadaMap);

            var puntos = (await this._serviceBase.GetAllAsync(e => e.CodRec == filtro.CodRec, new List<Expression<Func<PlaPuntos, object>>>() { e => e.PlaParada})).Items.OrderBy(e => e.Orden).ToList();

            List<PuntosDto> puntosdto = new List<PuntosDto>();

            MapObject(puntos, puntosdto);


            List<Position> positions = new List<Position>();


            foreach (var item in puntosdto)
            {
                DataInfoPuntos dataInfoPuntos = this.getInfoData(item);

                if (!item.EsPuntoInicio)
                {
                    foreach (var path in dataInfoPuntos.steps.SelectMany(e => e.path))
                    {
                        var positionpath = new Position(Math.Round(Convert.ToDouble(path.lat), 6), Math.Round(Convert.ToDouble(path.lng), 6));

                        positions.Add(positionpath);
                    }
                }

                var position = new Position(Math.Round(Convert.ToDouble(item.Lat),6), Math.Round(Convert.ToDouble(item.Long),6));

                positions.Add(position);
            }

            Folder folderRecorrido = new Folder();
            folderRecorrido.Name = string.Format("Recorrido - {0}", ruta.Bandera.AbrBan);

            SharpKml.Dom.LineString line = new SharpKml.Dom.LineString();
            line.Coordinates = this._sharpKML.CreateCoordinateCollection(positions);
            line.Tessellate = true;
            Placemark placeMark = this._sharpKML.CreatePlaceMark(ruta.Nombre, line);
            placeMark.StyleUrl = new Uri(String.Format("#{0}", linestringMap.Id), UriKind.Relative);
            folderRecorrido.AddFeature(placeMark);

            PuntoWithCoordenada primerpunto = new PuntoWithCoordenada();
            var primerpuntodto = puntosdto.FirstOrDefault();
            primerpunto.Lat = Math.Round(Convert.ToDouble(primerpuntodto.Lat), 6);
            primerpunto.Long = Math.Round(Convert.ToDouble(primerpuntodto.Long), 6);
            primerpunto.CoordenadaDescription = (await _coordenadasService.GetByIdAsync(primerpuntodto.PlaCoordenadaId.Value)).Descripcion;

            PuntoWithCoordenada segundopunto = new PuntoWithCoordenada();
            var segundopuntodto = puntosdto.LastOrDefault();
            segundopunto.Lat = Math.Round(Convert.ToDouble(segundopuntodto.Lat),6);
            segundopunto.Long = Math.Round(Convert.ToDouble(segundopuntodto.Long),6);
            segundopunto.CoordenadaDescription = (await _coordenadasService.GetByIdAsync(segundopuntodto.PlaCoordenadaId.Value)).Descripcion;


            await this._sharpKML.AddStartAndFinishIconsToDocument(primerpunto, segundopunto, folderRecorrido, StartMap, FinishMap);

            document.AddFeature(folderRecorrido);

            if (filtro.CambioSectoresMapa.Value == true)
            {
                Folder folderSectores = new Folder();
                folderSectores.Name = string.Format("Cambio de sector - {0}", ruta.Bandera.AbrBan);


                foreach (var item in puntosdto.Where(e => e.EsCambioSector == true))
                {

                    SharpKml.Dom.Point point = new SharpKml.Dom.Point();
                    point.Coordinate = new Vector(Math.Round(Convert.ToDouble(item.Lat),6),Math.Round(Convert.ToDouble(item.Long),6));

                    var desc = (await _coordenadasService.GetByIdAsync(item.PlaCoordenadaId.Value)).Descripcion;
                    Placemark placemark = this._sharpKML.CreatePlaceMark(desc, point);
                    placemark.StyleUrl = new Uri(String.Format("#{0}", PincheSectorMap.Id), UriKind.Relative);
                    folderSectores.AddFeature(placemark);

                }

                if (puntosdto.Where(e => e.EsCambioSector == true).Count() >= 1)
                {
                    document.AddFeature(folderSectores);
                }
            }

            if (filtro.ParadasMapa.Value == true)
            {
                Folder folderParadas = new Folder();
                folderParadas.Name = string.Format("Paradas - {0}", ruta.Bandera.AbrBan);

                foreach (var item in puntosdto.Where(e => e.EsParada == true && ((e.DropOffType == null && e.PickUpType == null) || (e.PickUpType == false && e.DropOffType == false))))
                {


                    SharpKml.Dom.Point point = new SharpKml.Dom.Point();
                    point.Coordinate = new Vector(Math.Round(Convert.ToDouble(item.Lat),6),Math.Round(Convert.ToDouble(item.Long),6));

                    Placemark placemark = this._sharpKML.CreatePlaceMark(item.PlaParadaCruceCalle?.ToUpperInvariant(), point);
                    placemark.StyleUrl = new Uri(String.Format("#{0}", PincheParadaMap.Id), UriKind.Relative);
                    folderParadas.AddFeature(placemark);

                }

                if (puntosdto.Where(e => e.EsParada == true).Count() >= 1)
                {
                    document.AddFeature(folderParadas);
                }

            }


            kml.Feature = document;

            KmlFile kmlfile = KmlFile.Create(kml, false);
            string filename = GetTempFileName("kml");

            using (FileStream fs = System.IO.File.Create(filename))
            {
            }

            using (var stream = System.IO.File.OpenWrite(filename))
            {
                kmlfile.Save(stream);
            }

            byte[] bytearray = System.IO.File.ReadAllBytes(filename);

            string estadomapa;
            if (ruta.EstadoRuta.Nombre.ToLower().Trim() == "aprobado")
            {
                if (ruta.EsOriginal == 0)
                {
                    estadomapa = string.Empty;
                }
                else
                {
                    estadomapa = "(Original)";
                }
            }
            else
            {
                estadomapa = "(Borrador)";
            }

            FileDto dto = new FileDto();
            dto.ByteArray = bytearray;
            dto.FileName = String.Format("{0} {1} {2} - {3} - {4} {5}.kml",
                                        ruta.Bandera.RamalColor != null ? ruta.Bandera.RamalColor.PlaLinea.DesLin.TrimOrNull() : "",
                                        ruta.Bandera.RamalColor != null ? ruta.Bandera.RamalColor.Nombre.TrimOrNull() : "",
                                        ruta.Bandera.AbrBan.TrimOrNull(),
                                        ruta.Fecha.ToString("yyyy.MM.dd"),
                                        ruta.Nombre.TrimOrNull(),
                                        estadomapa
                                        );
            dto.ForceDownload = true;

            return dto;

        }



        public static string GetTempFileName(string extension)
        {
            int attempt = 0;
            while (true)
            {
                string fileName = System.IO.Path.GetRandomFileName();
                fileName = System.IO.Path.ChangeExtension(fileName, extension);
                fileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);

                try
                {
                    using (new FileStream(fileName, FileMode.CreateNew)) { }
                    return fileName;
                }
                catch (IOException ex)
                {
                    if (++attempt == 10)
                        throw new IOException("No unique temporary file name is available.", ex);
                }
            }
        }




        private IEnumerable<PuntosDto> getPuntosBySector(PlaSector sector, List<PuntosDto> Puntos)
        {
            var pi = Puntos.Single(e => e.Id == sector.PuntoInicioId);
            var pf = Puntos.Single(e => e.Id == sector.PuntoFinId);

            return Puntos.Where(s => s.Orden > pi.Orden && s.Orden <= pf.Orden).ToList();
        }

        private GeoJSON.Net.Feature.Feature BuildSectores(List<PuntosDto> puntos, int key)
        {
            List<GeoJSON.Net.Geometry.Point> sectores = new List<GeoJSON.Net.Geometry.Point>();
            foreach (var item in puntos)
            {
                var position = new Position(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Long));
                if (item.EsCambioSector)
                {
                    sectores.Add(new GeoJSON.Net.Geometry.Point(position));
                }
            }
            MultiPoint multiPoint = new MultiPoint(sectores);

            Dictionary<string, object> properties = new Dictionary<string, object>();

            properties.Add("type", "sectores");

            properties.Add("key", key.ToString());

            properties.Add("icono", appUrlService.GetIconMarkerUrl(4));

            return new GeoJSON.Net.Feature.Feature(multiPoint, properties);
        }

        private GeoJSON.Net.Feature.Feature BuildParadas(List<PuntosDto> puntos, int key)
        {
            List<GeoJSON.Net.Geometry.Point> paradas = new List<GeoJSON.Net.Geometry.Point>();
            foreach (var item in puntos)
            {
                var position = new Position(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Long));
                if (item.EsParada)
                {
                    paradas.Add(new GeoJSON.Net.Geometry.Point(position));
                }
            }
            MultiPoint multiPoint = new MultiPoint(paradas);

            Dictionary<string, object> properties = new Dictionary<string, object>();

            properties.Add("type", "paradas");

            properties.Add("key", key.ToString());

            properties.Add("icono", appUrlService.GetIconMarkerUrl(2));

            return new GeoJSON.Net.Feature.Feature(multiPoint, properties);
        }


        private GeoJSON.Net.Feature.Feature BuildPuntoInicio(List<PuntosDto> puntos, int key)
        {
            List<GeoJSON.Net.Geometry.Point> paradas = new List<GeoJSON.Net.Geometry.Point>();
            foreach (var item in puntos)
            {
                var position = new Position(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Long));
                if (item.EsPuntoInicio)
                {
                    paradas.Add(new GeoJSON.Net.Geometry.Point(position));
                }
            }
            MultiPoint multiPoint = new MultiPoint(paradas);

            Dictionary<string, object> properties = new Dictionary<string, object>();

            properties.Add("type", "puntoInicial");

            properties.Add("key", key.ToString());

            properties.Add("icono", appUrlService.GetIconMarkerUrl(8));

            return new GeoJSON.Net.Feature.Feature(multiPoint, properties);
        }


        private GeoJSON.Net.Feature.Feature BuildPuntoFin(List<PuntosDto> puntos, int key)
        {
            List<GeoJSON.Net.Geometry.Point> paradas = new List<GeoJSON.Net.Geometry.Point>();
            foreach (var item in puntos)
            {
                var position = new Position(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Long));
                if (item.EsPuntoTermino)
                {
                    paradas.Add(new GeoJSON.Net.Geometry.Point(position));
                }
            }
            MultiPoint multiPoint = new MultiPoint(paradas);

            Dictionary<string, object> properties = new Dictionary<string, object>();

            properties.Add("type", "puntoFinal");

            properties.Add("key", key.ToString());

            properties.Add("icono", appUrlService.GetIconMarkerUrl(9));

            return new GeoJSON.Net.Feature.Feature(multiPoint, properties);
        }

        private List<GeoJSON.Net.Feature.Feature> BuildTramos(List<PuntosDto> puntos, int key)
        {
            List<TramoRecorrido> tramos = new List<TramoRecorrido>();

            string currenColor = "";//puntos[0].Color;


            var currenTramo = new TramoRecorrido();

            PuntosDto puntoAnterior = null;

            foreach (var punto in puntos)
            {
                if (punto.Color == currenColor)
                {
                    //continuo tramo
                    currenTramo.Puntos.Add(punto);

                }
                else
                {
                    //nuevo tramos
                    currenTramo = new TramoRecorrido();
                    //if (puntoAnterior!=null)
                    //{
                    //    currenTramo.Puntos.Add(puntoAnterior);
                    //}
                    currenTramo.Puntos.Add(punto);
                    currenTramo.Color = punto.Color;
                    currenColor = punto.Color;
                    tramos.Add(currenTramo);

                }


                puntoAnterior = punto;
            }


            List<GeoJSON.Net.Feature.Feature> features = new List<GeoJSON.Net.Feature.Feature>();

            foreach (var item in tramos)
            {
                Dictionary<string, object> properties = new Dictionary<string, object>();

                properties.Add("color", item.Color);

                properties.Add("type", "tramo");

                properties.Add("key", key.ToString());

                features.Add(new GeoJSON.Net.Feature.Feature(item.getLineString(), properties));

            }


            return features;


        }

        public async Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf)
        {
            return await this._serviceBase.GetFilterPuntosInicioFin(pf);
        }

        private class TramoRecorrido
        {

            public TramoRecorrido()
            {
                this.Puntos = new List<PuntosDto>();
            }

            public string Color { get; set; }

            public List<PuntosDto> Puntos { get; set; }

            internal GeoJSON.Net.Geometry.LineString getLineString()
            {
                List<Position> positions = new List<Position>();

                foreach (var item in this.Puntos)
                {

                    DataInfoPuntos dataInfoPuntos = this.getInfoData(item);

                    foreach (var path in dataInfoPuntos.steps.SelectMany(e => e.path))
                    {
                        var positionpath = new Position(Convert.ToDouble(path.lat), Convert.ToDouble(path.lng));

                        positions.Add(positionpath);
                    }

                    var position = new Position(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Long));

                    positions.Add(position);
                }

                return new GeoJSON.Net.Geometry.LineString(positions);
            }

            private DataInfoPuntos getInfoData(PuntosDto item)
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

        public DataInfoPuntos getInfoData(PuntosDto item)
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
