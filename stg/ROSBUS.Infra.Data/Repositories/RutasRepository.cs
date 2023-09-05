using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using Snickler.EFCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class RutasRepository : RepositoryBase<AdminContext, GpsRecorridos, int>, IRutasRepository
    {
        private IAdminDBResilientTransaction resilientTransaction;


        public RutasRepository(IAdminDbContext _context, IAdminDBResilientTransaction _resilientTransaction)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            this.resilientTransaction = _resilientTransaction;
        }
        
        public async Task<GpsRecorridos> TraerMinutosPorSector(MinutosPorSectorFilter filter)
        {
            try
            {
                IQueryable<GpsRecorridos> query = Context.GpsRecorridos.AsQueryable();

                query = query.Include(rec => rec.Sectores)
                    .Include(rec => rec.Puntos)
                    .ThenInclude(pun => pun.TipoParada)
                    .ThenInclude(tp => tp.PlaTiempoEsperadoDeCarga);

                var entity = await query.SingleAsync(GetFilterById(filter.Id));


                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(GpsRecorridos), (object)filter.Id);
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            if (resilientTransaction.IsResilientTransaction())
            {
                return Task.FromResult(0);
            }
            return base.SaveChangesAsync();
        }

        public async override Task<GpsRecorridos> GetByIdAsync<TFilter>(TFilter filter)
        {
            try
            {
                IQueryable<GpsRecorridos> query = Context.Set<GpsRecorridos>().AsQueryable();

                foreach (var include in filter.GetIncludesForGetById())
                {
                    var includeString = new FixVisitor(include).GetInclude();

                    query = query.Include(includeString);
                }

                query = query.Include(e => e.Puntos).ThenInclude(f => (f as PlaPuntos).PlaCoordenada);

                GpsRecorridos entity = await query.SingleAsync(GetFilterById(filter.Id));

                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(GpsRecorridos), (object)filter.Id);
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        protected override IQueryable<GpsRecorridos> AddIncludeForGet(DbSet<GpsRecorridos> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.EstadoRuta).Include(e => e.Bandera);
        }


        public override Expression<Func<GpsRecorridos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async override Task<GpsRecorridos> UpdateAsync(GpsRecorridos entity)
        {
            var OriginalEntity = Context.GpsRecorridos.AsNoTracking().FirstOrDefault(me => me.Id == entity.Id);

            using (var ts = await this.Context.Database.BeginTransactionAsync())
            {

                if (entity.EstadoRutaId == PlaEstadoRuta.APROBADO && OriginalEntity.EstadoRutaId == PlaEstadoRuta.BORRADOR)
                {
                    await AprobarRuta(entity);
                }
                else if (entity.EstadoRutaId != PlaEstadoRuta.APROBADO && OriginalEntity.EstadoRutaId == PlaEstadoRuta.APROBADO && entity.Fecha > DateTime.Now)
                {
                    DesaprobarRuta(entity);
                }

                var r = await base.UpdateAsync(entity);

                if (entity.EstadoRutaId == PlaEstadoRuta.APROBADO && OriginalEntity.EstadoRutaId == PlaEstadoRuta.BORRADOR)
                {
                    var l = this.Context.GpsDetaReco.Where(e => e.CodRec == entity.Id).ToList();
                    this.Context.RemoveRange(l);
                    await this.Context.SaveChangesAsync();

                    var gpsdatareco = CompletarDataReco(entity);

                    await this.Context.GpsDetaReco.AddRangeAsync(gpsdatareco);
                    await this.Context.SaveChangesAsync();


                    //FIX: 1120 no lo puedo reproducir vamos a agregar validaciones que validen el timeline de las fechas
                    var RutasDeBandera = Context.GpsRecorridos.Where(e => e.EstadoRutaId == PlaEstadoRuta.APROBADO && e.CodBan == entity.CodBan).ToList();

                    var fechasInvertidas = RutasDeBandera.Any(e => e.FechaVigenciaHasta != null && e.FechaVigenciaHasta < e.Fecha);
                    if (fechasInvertidas)
                    {
                        throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias. " +
                            "entre las fecha desde mayor a la fecha hasta");
                    }




                    foreach (var f in RutasDeBandera.OrderBy(e => e.Fecha))
                    {
                        if (RutasDeBandera.Any(e => e.Id != f.Id &&
                                        e.Fecha <= f.Fecha &&
                                        e.FechaVigenciaHasta >= f.Fecha &&
                                        e.FechaVigenciaHasta <= f.FechaVigenciaHasta))
                        {
                            throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias. " +
                         "entre la fecha desde se solapa con otra ruta");
                        }

                        if (RutasDeBandera.Any(e => e.Id != f.Id &&
                                       e.Fecha <= f.FechaVigenciaHasta &&
                                       e.Fecha >= f.Fecha &&
                                       e.FechaVigenciaHasta >= f.FechaVigenciaHasta
                                            ))
                        {
                            throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias. " +
                           "entre la fecha hasta se solapa con otra ruta");
                        }


                        if (RutasDeBandera.Any(e => e.Id != f.Id &&
                                    e.Fecha >= f.Fecha &&
                                    e.FechaVigenciaHasta <= f.FechaVigenciaHasta
                                         ))
                        {
                            throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias. " +
                           "se solapan las rutas");
                        }


                    }



                    var masDeUnRecoridoHastaNull = RutasDeBandera.Count(e => !e.FechaVigenciaHasta.HasValue);
                    if (masDeUnRecoridoHastaNull != 1)
                    {
                        throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias. " +
                         "solo puede exister un solo recorido sin fecha hasta");
                    }


                }

                await ActualizarEsOriginal(entity);
                //await ActualizarAnterior(entity, OriginalEntity);

                ts.Commit();

                return r;
            }


        }

        public async override Task<GpsRecorridos> AddAsync(GpsRecorridos entity)
        {
            try
            {
                DbSet<GpsRecorridos> dbSet = Context.Set<GpsRecorridos>();
                var entry = await dbSet.AddAsync(entity);
                await this.SaveChangesAsync();
                await ActualizarEsOriginal(entity);
                //await ActualizarAnterior(entity);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        private async Task ActualizarEsOriginal(GpsRecorridos entity)
        {
            var ruta = Context.GpsRecorridos.Where(me => me.CodBan == entity.CodBan && !me.IsDeleted
                                                                    && (me.EsOriginal == (int)GpsRecorridos.EnumOriginal.Original ||
                                                                        me.EsOriginal == (int)GpsRecorridos.EnumOriginal.AnteriorOriginal)).ToList();

            var rutaOriginal = Context.GpsRecorridos.Where(me => me.CodBan == entity.CodBan && !me.IsDeleted && me.EstadoRutaId == PlaEstadoRuta.APROBADO
                                                                    && (me.EsOriginal == (int)GpsRecorridos.EnumOriginal.Original ||
                                                                        me.EsOriginal == (int)GpsRecorridos.EnumOriginal.AnteriorOriginal)).OrderBy(e => e.Fecha).LastOrDefault();

            if (rutaOriginal!=null)
            {
                foreach (var item in ruta)
                {
                    if (item.Id == rutaOriginal.Id)
                    {
                        item.EsOriginal = (int)GpsRecorridos.EnumOriginal.Original;
                    }
                    else
                    {
                        item.EsOriginal = (int)GpsRecorridos.EnumOriginal.AnteriorOriginal;
                    }

                    this.Context.Entry(item).State = item.Id > 0 ? EntityState.Modified : EntityState.Added;
                }

                await this.Context.SaveChangesAsync();
            }

        }

        private async Task ActualizarAnterior(GpsRecorridos entity, GpsRecorridos OriginalEntity = null)
        {

            //si la entidad es nueva
            if (OriginalEntity == null)
            {                
                OriginalEntity = new GpsRecorridos();
            }

            if (OriginalEntity.EsOriginal != entity.EsOriginal)
            {
                //Paso a original
                if (OriginalEntity.EsOriginal == (int)GpsRecorridos.EnumOriginal.No && entity.EsOriginal == (int)GpsRecorridos.EnumOriginal.Original)
                {
                    var ruta = Context.GpsRecorridos.Where(me => me.CodBan == entity.CodBan && me.Id != entity.Id && !me.IsDeleted
                                                                    && me.EsOriginal == (int)GpsRecorridos.EnumOriginal.Original).FirstOrDefault();

                    //si existe un original guardado en base de datos
                    if (ruta != null)
                    {
                        //si mi entidad es mas nueva en el tiempo actualizo la anterior
                        if (entity.Fecha > ruta.Fecha)
                        {
                            ruta.EsOriginal = (int)GpsRecorridos.EnumOriginal.AnteriorOriginal;
                        }
                        else
                        {
                            entity.EsOriginal = (int)GpsRecorridos.EnumOriginal.AnteriorOriginal;
                        }
                        await this.Context.SaveChangesAsync();
                    }
                }
                //saco el original
                else if (OriginalEntity.EsOriginal == (int)GpsRecorridos.EnumOriginal.Original && entity.EsOriginal == (int)GpsRecorridos.EnumOriginal.No)
                {
                    var rutas = Context.GpsRecorridos.Where(me => me.CodBan == entity.CodBan && me.Id != entity.Id && !me.IsDeleted
                                                        && me.EsOriginal == (int)GpsRecorridos.EnumOriginal.AnteriorOriginal).OrderByDescending(e => e.Fecha).Take(1).FirstOrDefault();

                    //si existe un anterior original
                    if (rutas != null)
                    {
                        //lo pasamos como original
                        rutas.EsOriginal = (int)GpsRecorridos.EnumOriginal.Original;
                        await this.Context.SaveChangesAsync();
                    }
                }

                //saco anterior original
                //else if (OriginalEntity.EsOriginal == (int)GpsRecorridos.EnumOriginal.AnteriorOriginal && entity.EsOriginal == (int)GpsRecorridos.EnumOriginal.No)
                //{
                //    //entity.EsOriginal = (int)GpsRecorridos.EnumOriginal.No;

                //}
            }
        }


        public List<GpsDetaReco> CompletarDataReco(GpsRecorridos entity)
        {
            var detareco = entity.getDetaReco();

            var puntoanterior = detareco[0];
            var i = 0;

            var gpsdataredo = new List<GpsDetaReco>();

            foreach (var item in detareco)
            {
                if (i != 0)
                {
                    var lista = GenerarPuntosParaTecnoBus(Convert.ToDouble(puntoanterior.Lat), Convert.ToDouble(puntoanterior.Lon), Convert.ToDouble(item.Lat), Convert.ToDouble(item.Lon), 30);
                    if (lista.Any())
                    {
                        lista.Remove(lista.First());
                        lista.Remove(lista.Last());

                        foreach (var tup in lista)
                        {
                            var positionpath = new GpsDetaReco();
                            positionpath.Lat = Convert.ToDecimal(tup.Item1);
                            positionpath.Lon = Convert.ToDecimal(tup.Item2);
                            positionpath.CodRec = entity.Id;
                            positionpath.Sector = "0";
                            positionpath.DscSector = "";
                            if (!EsIgualAlUltimoPunto(gpsdataredo, positionpath))
                            {
                                gpsdataredo.Add(positionpath);
                            }


                        }

                    }



                }

                if (!EsIgualAlUltimoPunto(gpsdataredo, item))
                {
                    gpsdataredo.Add(item);
                }
                // correccion de ticket 2670
                else if (item.Sector == "1")
                {
                    gpsdataredo.Remove(gpsdataredo.Last());
                    gpsdataredo.Add(item);
                }


                i++;
                puntoanterior = item;
            }

            i = 1;



            puntoanterior = gpsdataredo[0];

            gpsdataredo.ForEach(f =>
            {

                f.Cuenta = i++;

                var dir = GetDirection(puntoanterior, f);

                f.Sent1 = dir.ToString();
                f.Sent2 = dir.ToString();
                f.Metro = Convert.ToDecimal(GetDistance(puntoanterior, f));
                puntoanterior = f;

            });

            gpsdataredo.ForEach(f =>
            {

                f.Lat = f.Lat * -1;
                f.Lon = f.Lon * -1;

            });


            return gpsdataredo;
        }

        private bool EsIgualAlUltimoPunto(List<GpsDetaReco> gpsdataredo, GpsDetaReco p)
        {
            var last = gpsdataredo.LastOrDefault();

            if (last != null)
            {
                return p.Lat == last.Lat && p.Lon == last.Lon;
            }

            return false;

        }

        public Double GetDistance(GpsDetaReco p1, GpsDetaReco p2)
        {
            var sCoord = new GeoCoordinate(Convert.ToDouble(p1.Lat), Convert.ToDouble(p1.Lon));
            var eCoord = new GeoCoordinate(Convert.ToDouble(p2.Lat), Convert.ToDouble(p2.Lon));

            var dist = sCoord.GetDistanceTo(eCoord);
            return dist;
        }


        public static Direction GetDirection(GpsDetaReco p1, GpsDetaReco p2)
        {
            double angle = Math.Atan2(Convert.ToDouble(p2.Lat) - Convert.ToDouble(p1.Lat), Convert.ToDouble(p2.Lon) - Convert.ToDouble(p1.Lon));
            angle += Math.PI;
            angle /= Math.PI / 4;
            int halfQuarter = Convert.ToInt32(angle);
            halfQuarter %= 8;
            return (Direction)halfQuarter;
        }

        public enum Direction
        {
            N,
            S,
            E,
            O,
            NE,
            NO,
            SE,
            SO,
            Undefined
        }


        private async Task AprobarRuta(GpsRecorridos entity)
        {
            GpsRecorridos NuevaRutaDuplicada = null;
            var RutasDeBandera = Context.GpsRecorridos.Where(e => e.EstadoRutaId == PlaEstadoRuta.APROBADO && e.CodBan == entity.CodBan).ToList();

            if (entity.FechaVigenciaHasta == null)
            {
                var RutaAnterior = RutasDeBandera.FirstOrDefault(e => e.Fecha <= entity.Fecha && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= entity.Fecha));

                var RutasAEliminar = RutasDeBandera.Where(e => e.Fecha >= entity.Fecha);


                if (RutaAnterior != null)
                {
                    RutaAnterior.FechaVigenciaHasta = entity.Fecha.AddDays(-1);
                    Context.Set<GpsRecorridos>().Update(RutaAnterior);
                } 
                
                foreach (var item in RutasAEliminar)
                {
                    //Fuerzo volver atraz para recuperar la fecha de vigencia ;
                    Context.Entry(item).Reload();
                    item.EstadoRutaId = PlaEstadoRuta.DESCARTADO;
                    Context.Set<GpsRecorridos>().Update(item);
                }
            }
            else
            {
                var RutaAnterior = RutasDeBandera.FirstOrDefault(e => e.Fecha <= entity.Fecha && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= entity.FechaVigenciaHasta));
                   // .Include(e => e.Puntos)
                    //.Include(e => e.Sectores)

                if (RutaAnterior != null)
                {
                    RutaAnterior = Context.GpsRecorridos.Include(e => e.Puntos).Include(e => e.Sectores).Single(e => e.Id == RutaAnterior.Id);

                    var FechaHasta = RutaAnterior.FechaVigenciaHasta;

                    RutaAnterior.FechaVigenciaHasta = entity.Fecha.AddDays(-1);
                    Context.Set<GpsRecorridos>().Update(RutaAnterior);

                    NuevaRutaDuplicada = new GpsRecorridos();
                    NuevaRutaDuplicada.Activo = "1";
                    NuevaRutaDuplicada.EstadoRutaId = RutaAnterior.EstadoRutaId;
                    NuevaRutaDuplicada.CodBan = RutaAnterior.CodBan;
                    NuevaRutaDuplicada.Fecha = entity.FechaVigenciaHasta.Value.AddDays(1);
                    NuevaRutaDuplicada.FechaVigenciaHasta = FechaHasta;
                    NuevaRutaDuplicada.Nombre = RutaAnterior.Nombre;
                    //NuevaRutaDuplicada.Puntos = RutaAnterior.Puntos; 

                    foreach (var item in RutaAnterior.Puntos)
                    {
                        var nuevopunto = item.Clone();
                        nuevopunto.CodRec = NuevaRutaDuplicada.Id;
                        nuevopunto.Ruta = NuevaRutaDuplicada;
                        nuevopunto.Id = Guid.NewGuid();
                        NuevaRutaDuplicada.Puntos.Add(nuevopunto);
                    }

                    foreach (var item in RutaAnterior.Sectores)
                    {
                        var PuntoInicioAnterior = RutaAnterior.Puntos.FirstOrDefault(e => e.Id == item.PuntoInicioId);
                        var PuntoFinAnterior = RutaAnterior.Puntos.FirstOrDefault(e => e.Id == item.PuntoFinId);
                        var sector = new PlaSector();

                        sector.Descripcion = sector.Descripcion;
                        sector.Data = sector.Data;
                        sector.DistanciaKm = sector.DistanciaKm;
                        sector.PuntoInicio = NuevaRutaDuplicada.Puntos.First(s => s.Orden == PuntoInicioAnterior.Orden);
                        sector.PuntoFin = NuevaRutaDuplicada.Puntos.First(s => s.Orden == PuntoFinAnterior.Orden);
                        sector.Ruta = NuevaRutaDuplicada;
                        sector.Color = sector.Color;
                        NuevaRutaDuplicada.Sectores.Add(sector);
                    }
                    Context.Set<GpsRecorridos>().Add(NuevaRutaDuplicada);
                    await this.Context.SaveChangesAsync();

                    var gpsdatareco = this.Context.GpsDetaReco.Where(e => e.CodRec == RutaAnterior.Id).ToList();
                    var gpsdatarecoDuplicada = gpsdatareco.Select(e => new GpsDetaReco()
                    {
                        CodRec = NuevaRutaDuplicada.Id,
                        Cuenta = e.Cuenta,
                        Sent1 = e.Sent1,
                        Sent2 = e.Sent2,
                        Metro = e.Metro,
                        Lat = e.Lat,
                        Lon = e.Lon,
                        Sector = e.Sector,
                        DscSector = e.DscSector,
                    });
                    await this.Context.GpsDetaReco.AddRangeAsync(gpsdatarecoDuplicada);
                    await this.Context.SaveChangesAsync();
                     

                   
                }

                var RutaEnElMediol = RutasDeBandera.Where(e => e.Fecha <= entity.Fecha && e.FechaVigenciaHasta >= entity.Fecha && e.FechaVigenciaHasta <= entity.FechaVigenciaHasta);
                var RutaEnElMedio = RutaEnElMediol.FirstOrDefault();
                if (RutaEnElMedio != null)
                {
                    RutaEnElMedio.FechaVigenciaHasta = entity.Fecha.AddDays(-1);
                    Context.Set<GpsRecorridos>().Update(RutaEnElMedio);
                }

                var RutaEnElMedio2l = RutasDeBandera.Where(e => e.Fecha > entity.Fecha && e.Fecha <= entity.FechaVigenciaHasta && e.FechaVigenciaHasta >= entity.FechaVigenciaHasta);
                var RutaEnElMedio2 = RutaEnElMedio2l.FirstOrDefault();
                if (RutaEnElMedio2 != null)
                {
                    RutaEnElMedio2.Fecha = entity.FechaVigenciaHasta.Value.AddDays(1);
                    Context.Set<GpsRecorridos>().Update(RutaEnElMedio2);
                }

                var RutasAEliminar = RutasDeBandera.Where(e => e.Fecha > entity.Fecha && e.FechaVigenciaHasta < entity.FechaVigenciaHasta);

                foreach (var item in RutasAEliminar)
                {
                    Context.Entry(item).Reload();                    
                    item.EstadoRutaId = PlaEstadoRuta.DESCARTADO;
                    Context.Set<GpsRecorridos>().Update(item);
                }
            } 
        }

        private void DesaprobarRuta(GpsRecorridos entity)
        {
            var RutaDeBandera = Context.GpsRecorridos.FirstOrDefault(e => e.EstadoRutaId == PlaEstadoRuta.APROBADO && e.CodBan == entity.CodBan && e.FechaVigenciaHasta.GetValueOrDefault().Date == entity.Fecha.AddDays(-1).Date);

            if (RutaDeBandera != null)
            {
                RutaDeBandera.FechaVigenciaHasta = entity.FechaVigenciaHasta;
                Context.Set<GpsRecorridos>().Update(RutaDeBandera);
            }
        }

        public async Task<List<HBasec>> RecuperarHbasecPorLinea(int cod_lin)
        {

            IList<HBasec> Results = new List<HBasec>(); ;


            var sp = this.Context.LoadStoredProc("dbo.sp_h_basec_RecuperarPorLinea")
                       .WithSqlParam("cod_lin", cod_lin);

            await sp.ExecuteStoredProcAsync((handler) =>
                     {
                         Results = handler.ReadToList<HBasec>();
                     });

            return Results.ToList();
        }



        public IList<Tuple<Double, Double>> GenerarPuntosParaTecnoBus(double latitude1, double longitude1, double latitude2, double longitude2, int distance)
        {
            var sCoord = new GeoCoordinate(latitude1, longitude1);
            var eCoord = new GeoCoordinate(latitude2, longitude2);

            var dist = sCoord.GetDistanceTo(eCoord);

            if (dist < 30)
            {
                return new List<Tuple<Double, Double>>();
            }

            return SplitLine(new Tuple<double, double>(latitude1, longitude1), new Tuple<double, double>(latitude2, longitude2), (int)Math.Ceiling(((double)dist) / distance));
        }

        private static IList<Tuple<Double, Double>> SplitLine(Tuple<Double, Double> a, Tuple<Double, Double> b, int count)
        {
            count = count + 1;

            Double d = Math.Sqrt((a.Item1 - b.Item1) * (a.Item1 - b.Item1) + (a.Item2 - b.Item2) * (a.Item2 - b.Item2)) / count;
            Double fi = Math.Atan2(b.Item2 - a.Item2, b.Item1 - a.Item1);

            List<Tuple<Double, Double>> points = new List<Tuple<Double, Double>>(count + 1);

            for (int i = 0; i <= count; ++i)
                points.Add(new Tuple<Double, Double>(a.Item1 + i * d * Math.Cos(fi), a.Item2 + i * d * Math.Sin(fi)));

            return points;
        }

        public async Task<int?> GetEstadoOriginal(int id)
        {
            GpsRecorridos Recorrido = new GpsRecorridos();
            var sp = this.Context.LoadStoredProc("dbo.gps_GetRutaEstadoOriginal")
                                  .WithSqlParam("cod_rec", id);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Recorrido = handler.ReadToList<GpsRecorridos>().FirstOrDefault();
            });

            return Recorrido.EstadoRutaId;
        }

        public async Task<GpsRecorridos> UpdateInstructions(GpsRecorridos entity)
        {
                try
                {
 
                    this.Context.Entry(entity).State = EntityState.Modified;

                    await this.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            return entity;
        }
    }
}
