using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
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

namespace ROSBUS.Admin.AppService
{

    public class RutasAppService : AppServiceBase<GpsRecorridos, RutasDto, int, IRutasService>, IRutasAppService
    {
        private readonly IAdminDBResilientTransaction resilientTransaction;
        private readonly ICoordenadasService coordenadasService;
        private readonly ITiposDeDiasService tipodiaService;
        private readonly IBandasHorariasService bandasHorariasService;
        private readonly IHFechasService hfechaservice;
        private readonly IHMinxtipoService hMinxtipoService;

        public RutasAppService(IRutasService serviceBase, IAdminDBResilientTransaction ResilientTransaction,
            ICoordenadasService _coordenadasService, ITiposDeDiasService _tipodiaservice,
            IBandasHorariasService _bandasHorariasService, IHFechasService _hfechaservice,
            IHMinxtipoService _hMinxtipoService)
            : base(serviceBase)
        {
            resilientTransaction = ResilientTransaction;
            this.coordenadasService = _coordenadasService;
            this.tipodiaService = _tipodiaservice;
            this.bandasHorariasService = _bandasHorariasService;
            this.hfechaservice = _hfechaservice;
            this.hMinxtipoService = _hMinxtipoService;
        }


        private void FixToDateDto(RutasDto dto)
        {
            dto.FechaVigenciaDesde = dto.FechaVigenciaDesde.Value.Date;
            if (dto.FechaVigenciaHasta.HasValue)
            {
                dto.FechaVigenciaHasta = dto.FechaVigenciaHasta.Value.Date;
            }
        }

        public async override Task<RutasDto> UpdateAsync(RutasDto dto)
        {
            RutasFilter filter = new RutasFilter();
            filter.Id = dto.Id;

            this.FixToDateDto(dto);


            if (dto.Puntos != null)
            {
                foreach (var item in dto.Puntos)
                {
                    item.RutaId = dto.Id;
                }
            }

            var entity = await this.GetByIdAsync(filter);
 

            if (entity.EstadoRutaId == PlaEstadoRuta.APROBADO)
            {
                entity.Instructions = dto.Instructions;
                await this._serviceBase.UpdateInstructions(entity);
            }
            else
            {

                MapObject(dto, entity);

                foreach (var item in entity.Puntos)
                {
                    item.CodRec = dto.Id;
                }

                this.UnificarSectorTarifario(entity);

                await this.UpdateAsync(entity);
            }
             

            return MapObject<GpsRecorridos, RutasDto>(entity);
        }

        private async Task GenerarNuevasCoordenadas(GpsRecorridos entity)
        {
            foreach (var item in entity.Puntos.Where(e => e.EsCambioSector || e.EsPuntoInicio || e.EsPuntoTermino))
            {
                CoordenadasFilter cf = new CoordenadasFilter();
                cf.Abreviacion = item.Abreviacion;
                cf.CodigoNombre = item.CodigoNombre;

                if (!this.coordenadasService.ExistExpression(cf.GetFilterExpression()))
                {
                    var nuevacordenada = new PlaCoordenadas();
                    nuevacordenada.Abreviacion = item.Abreviacion;
                    nuevacordenada.CodigoNombre = item.CodigoNombre;
                    nuevacordenada.Lat = item.Lat;
                    nuevacordenada.Long = item.Long;
                    // nuevacordenada.Id = Guid.NewGuid();

                    await this.coordenadasService.AddAsync(nuevacordenada);
                }
            }
        }

        private async Task<GpsRecorridos> GetRutaOriginal(RutasDto dto)
        {
            RutasFilter Filtroesoriginal = new RutasFilter();
            Filtroesoriginal.BanderaId = dto.BanderaId;
            //  Filtroesoriginal.EstadoRutaId = PlaEstadoRuta.APROBADO;
            Filtroesoriginal.EsOriginal = dto.EsOriginal == 1 ? (int)GpsRecorridos.EnumOriginal.Original : (int)GpsRecorridos.EnumOriginal.No;
            return (await this.GetAllAsync(Filtroesoriginal.GetFilterExpression())).Items.FirstOrDefault(); ;
        }

        public async override Task<RutasDto> AddAsync(RutasDto dto)
        {
            this.FixToDateDto(dto);

            if (!dto.CodSec.HasValue)
            {
                dto.CodSec = 0;
            }

            if (dto.CopyFromRutaId != null)
            {
                RutasFilter filter = new RutasFilter();
                filter.Id = dto.CopyFromRutaId.Value;
                var copy = await this.GetByIdAsync(filter);

                dto.Sectores = new List<SectorDto>();
                dto.Puntos = new List<PuntosDto>();

                if (copy.Sectores != null)
                {
                    foreach (var item in copy.Sectores)
                    {
                        var pdt = MapObject<PlaSector, SectorDto>(item);
                        pdt.Id = 0;
                        dto.Sectores.Add(pdt);
                    }
                }

                if (copy.Puntos != null)
                {
                    foreach (var item in copy.Puntos)
                    {
                        var pdt = MapObject<PlaPuntos, PuntosDto>(item);
                        var newid = Guid.NewGuid();

                        if (dto.Sectores != null)
                        {
                            foreach (var s in dto.Sectores.Where(e => e.PuntoInicioId == pdt.Id))
                            {
                                s.PuntoInicioId = newid;
                            }
                            foreach (var s in dto.Sectores.Where(e => e.PuntoFinId == pdt.Id))
                            {
                                s.PuntoFinId = newid;
                            }
                        }

                        pdt.Id = newid;

                        //if (item.PlaCoordenadaId.HasValue && item.PlaCoordenada.Anulado)
                        //{
                        //    throw new ValidationException("No puede copiar un mapa que contenga una coordenada anulada");
                        //}

                        dto.Puntos.Add(pdt);
                    }

                    
                }

            }



            var entity = MapObject<RutasDto, GpsRecorridos>(dto);

            this.UnificarSectorTarifario(entity);

            return MapObject<GpsRecorridos, RutasDto>(await this.AddAsync(entity));
        }

        private void UnificarSectorTarifario(GpsRecorridos entity)
        {
            int? codSectorTarifario = null;
            foreach (var item in entity.Puntos.Where(w => w.EsCambioSector || w.EsPuntoInicio || w.EsPuntoTermino).OrderBy(e => e.Orden))
            {
                if (item.EsCambioSectorTarifario)
                {
                    codSectorTarifario = item.CodSectorTarifario;
                }
                else
                {
                    item.CodSectorTarifario = codSectorTarifario;
                }
            }

        }



        public async Task<RutasDto> AprobarRutaAsync(int id)
        {
            var item = await this.GetByIdAsync(id);

            if (item.EstadoRutaId == PlaEstadoRuta.BORRADOR)
                item.EstadoRutaId = PlaEstadoRuta.APROBADO;

            return MapObject<GpsRecorridos, RutasDto>(await this.UpdateAsync(item));
        }

        public async Task<List<string>> ValidateAprobarRuta(int id)
        {
            var entity = await this.GetByIdAsync(id);

            return await this._serviceBase.ValidateAprobarRuta(entity);
        }


        public async Task<List<string>> ValidateAprobarRuta(RutasDto dto)
        {
            var entity = MapObject<RutasDto, GpsRecorridos>(dto);

            return await this._serviceBase.ValidateAprobarRuta(entity);
        }

        public async Task<bool> CanDeleteGalpon(GalponDto taller)
        {
            //RutasFilter rf = new RutasFilter() { UnidadDeNegocioId = taller.UnidadDeNegocioId };
            RutasFilter rf = new RutasFilter() { };
            rf.Abreviacion = taller.Nombre;

            return await Task.FromResult(!this._serviceBase.ExistExpression(rf.GetFilterRutasPosicionamiento()));
        }
        public async Task<List<RutasDto>> GetRutas(RutasViewFilter filter)
        {

            var filterExp = new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Puntos,
                    e=> e.Sectores,
                    e=> e.Bandera
                };

            var rutasentiti = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filterExp);

            var dtplist = this.MapList<GpsRecorridos, RutasDto>(rutasentiti.Items);

            return dtplist.ToList();
        }

        public async Task<List<RutasDto>> GetRutasFiltradas(RutasFilteredFilter filter)
        {
            List<HSectores> sectoresorig = new List<HSectores>();
            List<RutasDto> showroute = new List<RutasDto>();
            HMinxtipoFilter sector = new HMinxtipoFilter();
            sector.CodBan = filter.BanderaId;
            sector.CodHfecha = filter.CodHFecha;
            sectoresorig = await this.hMinxtipoService.GetHSectores(sector);
            sectoresorig = sectoresorig.OrderBy(e => e.Orden).ToList();

            var filterExp = new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Puntos,
                    e=> e.Sectores,
                    e=> e.Bandera
                };

            var rutasentity = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filterExp);

            var recorridos = this.MapList<GpsRecorridos, RutasDto>(rutasentity.Items);
            foreach (var recorrido in recorridos)
            {
                var sectoresnew = recorrido.Puntos.Where(e => e.EsCambioSector == true || e.EsPuntoInicio == true || e.EsPuntoTermino == true).OrderBy(e => e.Orden).ToList();
                var show = true;
                if (sectoresorig.Count == sectoresnew.Count)
                {
                    var orden = 0;
                    foreach (var sectornew in sectoresnew)
                    {
                        orden++;
                        var equalsec = sectoresorig.Where(e => e.CodHsectorNavigation.PlaPuntos.Any(p => p.Id == sectornew.Id) && e.Orden == orden && e.CodHsectorNavigation.Id == sectornew.PlaCoordenadaId).FirstOrDefault();
                        if (equalsec == null)
                        {
                            show = false;
                            break;
                        }
                    }

                    if (show)
                    {
                        var showrec = recorridos.Where(e => e.Id == recorrido.Id).FirstOrDefault();
                        showroute.Add(showrec);
                    }
                }
                else
                {
                    show = false;
                }
            }
 
            return showroute;
        }

        public async Task<List<ItemDto>> RecuperarHbasecPorLinea(int cod_lin)
        {
            if (cod_lin == 0)
            {
                throw new ValidationException("Parametros incorrectos, falta codigo linea");

            }
            var Hbase = await this._serviceBase.RecuperarHbasecPorLinea(cod_lin);
            List<ItemDto> result = new List<ItemDto>();


            foreach (var item in Hbase)
            {
                result.Add(new ItemDto(item.CodBan, item.CodBan.ToString()));
            }

            return result;
        }

        public async Task<PuntoBandasHorariasDto> MinutosPorSector(MinutosPorSectorFilter filter)
        {

            var ruta = await this._serviceBase.TraerMinutosPorSector(filter);
            var dto = new PuntoBandasHorariasDto();

            dto.TipoDia = (new List<HTipodia>()
            {
                await tipodiaService.GetByIdAsync(filter.TipoDiaId)
            }
            )
            .ToDictionary(
                d => d.Id,
                d => d.DesTdia);

            dto.Franjas = (await this.bandasHorariasService.GetAllAsync(e => true)).Items
                .ToDictionary(f => f.Id, f => new Tuple<TimeSpan, TimeSpan>(f.HoraDesde, f.HoraHasta));

            foreach (var sector in ruta.Sectores)
            {
                var demora = new PuntoBandasHorariasRowDto();
                demora.PuntoInicio = sector.PuntoInicio.PlaCoordenada.NumeroExternoIVU;
                demora.PuntoFin = sector.PuntoFin.PlaCoordenada.NumeroExternoIVU;
                demora.Largo = sector.DistanciaKm;

                foreach (var tipodia in dto.TipoDia)
                {
                    foreach (var franja in dto.Franjas)
                    {
                        //Sumar demora en subir gente en las paradas

                        var dtominutos = new DemoraBandasHorariasDto()
                        {
                            Demora = TimeSpan.FromSeconds(0),
                            Franja = franja.Key,
                            TipoDia = tipodia.Key
                        };

                        var demora_total = ruta.Puntos
                            .Where(p => p.EsParada && p.Orden > sector.PuntoInicio.Orden
                        && p.Orden <= sector.PuntoFin.Orden && p.EsParada)
                        .Select(p => p.TipoParada.PlaTiempoEsperadoDeCarga.Where(e =>
                        e.HoraDesde > franja.Value.Item1
                        && e.HoraHasta <= franja.Value.Item2
                        && e.TipodeDiaId == tipodia.Key).Sum(e => e.TiempoDeCarga.TotalSeconds)).Sum(e => e);

                        dtominutos.Demora += TimeSpan.FromSeconds(demora_total);

                        //Sumar tiempo de recorrido
                        var demora_viaje = sector.PlaMinutosPorSector.FirstOrDefault(min => min.IdBandaHoraria == franja.Key);
                        if (demora_viaje != null)
                        {
                            dtominutos.Demora += demora_viaje.Demora;
                        }

                        demora.Demoras.Add(dtominutos);
                    }
                }
                dto.Filas.Add(demora);
            }

            if (filter.SectoresIds.Count == 1)
            {
                var fila = dto.Filas.First();
                for (int i = fila.Demoras.Count - 1; i > 0; i--)
                {
                    if (fila.Demoras[i].Demora == fila.Demoras[i - 1].Demora)
                    {
                        // Junto las dos columnas
                        var alta = dto.Franjas[fila.Demoras[i].Franja];
                        var baja = dto.Franjas[fila.Demoras[i - 1].Franja];
                        dto.Franjas[fila.Demoras[i - 1].Franja] = new Tuple<TimeSpan, TimeSpan>(baja.Item1, alta.Item2);
                        dto.Franjas.Remove(fila.Demoras[i].Franja);
                        fila.Demoras.RemoveAt(i);
                    }

                }
            }
            return dto;
        }

        public async Task<FileDto> MinutosPorSectorReporte(MinutosPorSectorFilter filter)
        {
            var dto = await this.MinutosPorSector(filter);

            var exl = new ExcelParameters<PuntoBandasHorariasDto>();
            ExcelPackage paq = new ExcelPackage();
            var hoja = paq.Workbook.Worksheets.Add("Minutos por Sector");

            int row = 2;
            int col = 1;

            hoja.Cells[row, col].Value = "Punto de inicio";
            col++;
            hoja.Cells[row, col].Value = "Punto de Término";
            col++;
            hoja.Cells[row, col].Value = "Largo";
            col++;


            foreach (var dia in dto.TipoDia)
            {
                row = 1;
                foreach (var franja in dto.Franjas)
                {
                    hoja.Cells[row, col].Value = dia.Value.ToString();
                    hoja.Cells[row + 1, col].Value = string.Format("{0} - {1}", franja.Value.Item1.ToString("c"), franja.Value.Item2.ToString("c"));
                    col++;
                }
            }

            int matrix_begin_col = 4;
            int matrix_begin_row = 3;


            row = matrix_begin_row;

            for (int limitrow = row + dto.Filas.Count, initialrow = row; row < limitrow; row++)
            {
                col = 1;
                hoja.Cells[row, col].Value = dto.Filas[row - initialrow].PuntoInicio;
                col++;
                hoja.Cells[row, col].Value = dto.Filas[row - initialrow].PuntoFin;
                col++;
                hoja.Cells[row, col].Value = dto.Filas[row - initialrow].Largo ?? 0;
                col = matrix_begin_col;
                for (int limitcol = col + dto.Filas[row - initialrow].Demoras.Count, initialcol = col; col < limitcol; col++)
                {
                    hoja.Cells[row, col].Value = dto.Filas[row - initialrow].Demoras[col - initialcol].Demora.TotalSeconds.ToString();
                }
            }

            var filedto = new FileDto();

            filedto.ByteArray = ExcelHelper.GenerateByteArray(paq);
            filedto.ForceDownload = true;
            filedto.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            filedto.FileName = string.Format("Demoras-Ruta-{0}.xlsx", filter.Id);
            filedto.FileDescription = "Archivo de prueba";

            return filedto;
        }
    }
}
