using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;
using System.Linq;
using TECSO.FWK.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ROSBUS.Admin.Domain.Services
{
    public class RutasService : ServiceBase<GpsRecorridos, int, IRutasRepository>, IRutasService
    {
        private readonly IPlaMinutosPorSectorRepository _minutosRepository;
        private readonly IHFechasRepository _fechaRepository;
        private readonly IPuntosRepository _puntosRepository;


        public RutasService(IRutasRepository rutasRepository, IBanderaRepository banderaRepository, IHFechasRepository fechaRepository,
            IPlaMinutosPorSectorRepository minutosRepository, IPuntosRepository puntosRepository)
            : base(rutasRepository)
        {
            _banderaRepository = banderaRepository;
            _minutosRepository = minutosRepository;
            _fechaRepository = fechaRepository;
            _puntosRepository = puntosRepository;
      
        }

        private readonly IBanderaRepository _banderaRepository;


        public override Task<GpsRecorridos> AddAsync(GpsRecorridos entity)
        {
            entity.EstadoRutaId = PlaEstadoRuta.BORRADOR;

            return base.AddAsync(entity);
        }

        protected async override Task ValidateEntity(GpsRecorridos entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);

            HBanderas bandera = entity.Bandera;
            if (bandera == null)
            {
                bandera = this._banderaRepository.GetById(entity.CodBan);
            }


            if (entity.FechaVigenciaHasta.HasValue &&  entity.Fecha.Date >= entity.FechaVigenciaHasta.Value.Date)
            {
                throw new ValidationException("Fecha hasta debe ser mayor a fecha desde.");
              
            }

            if (mode == SaveMode.Update)
            {
                var diferencia = entity.Puntos.Where(e => !string.IsNullOrEmpty(e.CodigoNombre)).Count() != entity.Puntos.Where(e => !string.IsNullOrEmpty(e.CodigoNombre)).GroupBy(e => e.CodigoNombre).Count();
                if (diferencia)
                {
                    var cd = entity.Puntos.GroupBy(e => e.CodigoNombre).Where(g => g.Count() > 1).Select(s => s.Key).ToArray();
                    throw new DomainValidationException(string.Format("Existen codigos duplicado/s;  {0}", string.Join(";", cd)));
                }
            }


            int? rutaorig = null;
            if (entity.Id != 0)
            {
                rutaorig = await this.repository.GetEstadoOriginal(entity.Id);
            }

            if (!rutaorig.HasValue || (rutaorig != null && rutaorig != entity.EstadoRutaId) || entity.Puntos.Count == 0)
            {
                if (mode == SaveMode.Update && entity.EstadoRutaId == PlaEstadoRuta.APROBADO)
                {
                    var rutas = this.repository.GetAllAsync(e => e.CodBan == entity.CodBan && e.EstadoRutaId == PlaEstadoRuta.APROBADO).Result;
                    var RutasDeBandera = rutas.Items;



                    if (entity.Puntos.Any())
                    {
                        entity.ValidarPuntosEnEstadoaprobado(entity.Puntos, bandera.TipoBanderaId);
                    }
                    else
                    {
                        var puntos = this._puntosRepository.GetAll(e => e.CodRec == entity.Id).Items;

                        entity.ValidarPuntosEnEstadoaprobado(puntos, bandera.TipoBanderaId);
                    }

                    if (rutas.TotalCount == 0 && entity.FechaVigenciaHasta != null)
                        throw new DomainValidationException("La Fecha de Fin de Vigencia no debe ser completada debido a que aún no existen rutas aprobadas para esta bandera");

                    if (rutas.TotalCount > 0 && entity.Fecha.Date <= DateTime.Now.Date)
                        throw new DomainValidationException("La Fecha de Inicio de Vigencia debe mayor a hoy");

                    var confirmList = new List<string>();

                }
            }

            
        }

        public async Task<List<string>> ValidateAprobarRuta(GpsRecorridos entity)
        {
            var rutas = this.repository.GetAllAsync(e => e.CodBan == entity.CodBan && e.EstadoRutaId == PlaEstadoRuta.APROBADO).Result;
            var RutasDeBandera = rutas.Items;

            HBanderas bandera = entity.Bandera;
            if (bandera==null)
            {
                bandera = this._banderaRepository.GetById(entity.CodBan);
            }

            //if (rutas.TotalCount == 0 && entity.Fecha.Date > DateTime.Now.Date)
            //    throw new DomainValidationException("La Fecha de Inicio de Vigencia debe ser anterior o igual a hoy debido a que aún no existen rutas aprobadas para esta bandera");

            if (rutas.TotalCount == 0 && entity.FechaVigenciaHasta != null)
                throw new DomainValidationException("La Fecha de Fin de Vigencia no debe ser completada debido a que aún no existen rutas aprobadas para esta bandera");

            if (rutas.TotalCount > 0 && entity.Fecha.Date <= DateTime.Now.Date)
                throw new DomainValidationException("La Fecha de Inicio de Vigencia debe mayor a hoy");



            if (entity.Puntos.Any())
            {
                entity.ValidarPuntosEnEstadoaprobado(entity.Puntos, bandera.TipoBanderaId);
            }
            else
            {
                var puntos = this._puntosRepository.GetAll(e => e.CodRec == entity.Id).Items;

                entity.ValidarPuntosEnEstadoaprobado(puntos, bandera.TipoBanderaId);
            }

            var confirmList = new List<string>();

            if (entity.FechaVigenciaHasta == null)
            {
                var RutaAnterior = rutas.Items.FirstOrDefault(e => e.Fecha <= entity.Fecha && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= entity.Fecha));

                if (RutaAnterior != null)
                {
                    confirmList.Add(string.Format("La ruta que se intenta aprobar se superpone con la ruta: '{0}'. Confirme para actualizar dicha ruta ", RutaAnterior.Nombre));
                }

                var RutasAEliminar = rutas.Items.Where(e => e.Fecha >= entity.Fecha);

                if (RutasAEliminar.Any())
                {
                    confirmList.Add(string.Format("Se descartaran las rutas: {0}", string.Join('-', RutasAEliminar.Select(e => e.Nombre).ToArray())));
                }
            }
            else
            {
                                                                 
                var RutaAnterior = RutasDeBandera.FirstOrDefault(e => e.Fecha <= entity.Fecha && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= entity.FechaVigenciaHasta));

                if (RutaAnterior != null)
                {
                    confirmList.Add(string.Format("La ruta '{0}' se duplicara a partir de la fecha vigencia hasta {1}", RutaAnterior.Nombre, entity.FechaVigenciaHasta.ToStringDefaultFormat()));
                }
                                                                                  
                var RutaEnElMedio = RutasDeBandera.FirstOrDefault(e => e.Fecha <= entity.Fecha && e.FechaVigenciaHasta >= entity.Fecha &&  e.FechaVigenciaHasta <= entity.FechaVigenciaHasta);

                if (RutaEnElMedio != null)
                {
                    confirmList.Add(string.Format("La ruta que se intenta aprobar se superpone con la ruta: '{0}'. Confirme para actualizar dicha ruta ", RutaEnElMedio.Nombre));
                }

                var RutaEnElMedio2 = RutasDeBandera.FirstOrDefault(e => e.Fecha > entity.Fecha &&  e.Fecha <= entity.FechaVigenciaHasta && e.FechaVigenciaHasta >= entity.FechaVigenciaHasta);
                if (RutaEnElMedio2 != null)
                {
                    confirmList.Add(string.Format("La ruta que se intenta aprobar se superpone con la ruta: '{0}'. Confirme para actualizar dicha ruta ", RutaEnElMedio2.Nombre));
                }

                var RutasAEliminar = RutasDeBandera.Where(e => e.Fecha > entity.Fecha && e.FechaVigenciaHasta < entity.FechaVigenciaHasta);

                if (RutasAEliminar.Any())
                {
                    confirmList.Add(string.Format("Se descartaran las rutas: {0}", string.Join('-', RutasAEliminar.Select(e => e.Nombre).ToArray())));
                }


            }

            return await Task.FromResult(confirmList);
        }

      

        public async Task<List<HBasec>> RecuperarHbasecPorLinea(int cod_lin)
        {
            return await this.repository.RecuperarHbasecPorLinea(cod_lin);
        }


        public async Task<GpsRecorridos> TraerMinutosPorSector(MinutosPorSectorFilter filter)
        {
            var rec = await this.repository.TraerMinutosPorSector(filter);
            rec.Sectores = rec.Sectores.Where(s => filter.SectoresIds.Contains(s.Id)).ToList();
            var fecha = await _fechaRepository.GetHFechaAsync(new HFechasFilter() { RutaID = filter.Id, TipoDiaID = filter.TipoDiaId });
            foreach (var sec in rec.Sectores)
            {
                var minfilter = new PlaMinutosPorSectorFilter() { IdSector = sec.Id, Fecha = fecha.Fecha };
                sec.PlaMinutosPorSector = (await _minutosRepository.GetAllAsync(minfilter.GetFilterExpression())).Items.ToList();
            }
            return rec;
        }

        public async Task<GpsRecorridos> UpdateInstructions(GpsRecorridos entity)
        {
            return await this.repository.UpdateInstructions(entity);
        }
    }

}
