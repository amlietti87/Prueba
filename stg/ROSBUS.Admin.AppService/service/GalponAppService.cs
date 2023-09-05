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
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService
{

    public class GalponAppService : AppServiceBase<Galpon, GalponDto, Decimal, IGalponService>, IGalponAppService
    {
        private readonly IPuntosAppService puntosAppService;
        private readonly IRutasAppService rutasservice;
        private readonly IBanderaService banderaservice;

        public GalponAppService(IGalponService serviceBase, IPuntosAppService _puntosAppService, IRutasAppService _rutasservice, IBanderaService banderaservice)
            : base(serviceBase)
        {
            this.puntosAppService = _puntosAppService;
            this.rutasservice = _rutasservice;
            this.banderaservice = banderaservice;
        }

        public async Task<bool> CanDeleteGalpon(GalponDto taller)
        {
            

            return (await this.rutasservice.CanDeleteGalpon(taller));
        }


        public override async Task DeleteAsync(decimal id)
        {

            var dto = await this.GetDtoByIdAsync(id);

            if (!(await this.CanDeleteGalpon(dto)))
            {
                throw new ValidationException("No se puede eliminar el taller, porque tiene rutas asocia.");
            }

            await base.DeleteAsync(id);
        }

        public async Task<List<RutasDto>> GetPuntosInicioFin(GalponFilter filter)
        {


            if (string.IsNullOrEmpty(filter.Nombre))
            {
                return new List<RutasDto>();
            }

            //if (!filter.UnidadDeNegocioId.HasValue)
            //{
            //    throw new ValidationException("Falta Taller");
            //}

            var idUN = filter.SucursalId;
            PuntosFilter pf = new PuntosFilter() { UnidadDeNegocioId = filter.SucursalId };

            List<PlaPuntos> result = await this.puntosAppService.GetFilterPuntosInicioFin(pf);
            //var result2 = await this.puntosAppService.GetAllAsync(pf.GetFilterPuntosInicioFin());
            var puntosDtos = this.MapList<PlaPuntos, PuntosDto>(result);


            RutasFilter rf = new RutasFilter() { SucursalId = filter.SucursalId };
            rf.Abreviacion = filter.Nombre;
            rf.AllIncludes = true;

            var rutas = (await this.rutasservice.GetAllAsync(rf.GetFilterRutasPosicionamiento(), rf.GetIncludesForPageList()));
            var rutasdtop = this.MapList<GpsRecorridos, RutasDto>(rutas.Items).ToList();

            foreach (var item in puntosDtos)
            {
                //si no existe bandera de pocicionamiento cargada creo una por defecto
                if (!rutasdtop.Any(r => r.Puntos.Any(e => e.Abreviacion == item.Abreviacion && e.CodigoNombre == item.CodigoNombre)))
                {
                    var r = new RutasDto();
                    r.Puntos = new List<PuntosDto>();
                    
                    if (item.EsPuntoInicio)
                    {
                        r.Puntos.Add(new PuntosDto()
                        {                           
                            Id = Guid.NewGuid(),
                            EsPuntoInicio = true,
                            Lat = filter.Lat,
                            Long = filter.Long,
                            CodigoNombre = filter.Nombre,
                            Abreviacion = filter.Nombre,
                        }); //taller

                        r.Puntos.Add(new PuntosDto()
                        {
                            Id = Guid.NewGuid(),
                            EsPuntoTermino = true,
                            Lat = item.Lat,
                            Long = item.Long,
                            CodigoNombre = item.CodigoNombre,
                            Abreviacion = item.Abreviacion,
                        }); //punto
                    }
                    else if (item.EsPuntoTermino)
                    {
                        r.Puntos.Add(new PuntosDto()
                        {
                            Id = Guid.NewGuid(),
                            EsPuntoInicio = true,
                            Lat = item.Lat,
                            Long = item.Long,
                            CodigoNombre = item.CodigoNombre,
                            Abreviacion = item.Abreviacion,
                        }); //punto

                        r.Puntos.Add(new PuntosDto()
                        {
                            Id = Guid.NewGuid(),
                            EsPuntoTermino = true,
                            Lat = filter.Lat,
                            Long = filter.Long,
                            CodigoNombre = filter.Nombre,
                            Abreviacion = filter.Nombre,
                        }); //taller                       
                    }


                    r.Nombre = string.Format("{0} {1}", r.Puntos.First(e => e.EsPuntoInicio).Abreviacion, r.Puntos.First(e => e.EsPuntoTermino).Abreviacion);
                    r.CodigoVarianteLinea = item.CodigoVarianteLinea;

                    rutasdtop.Add(r);
                }
            }


            return rutasdtop.ToList();
        }

        public async Task SaveGalponPorUnidadDeNegocio(List<GalponDto> galpones)
        {
            //TODO: RC
            var filter = new GalponFilter();

            var originalList = this.GetAll(filter.GetFilterExpression()).Items;
            var talleres = this.MapList<GalponDto, Galpon>(galpones, originalList);


            foreach (var item in talleres)
            {
                if (item.PosGal.IsNullOrEmpty())
                {
                    item.PosGal = "";
                }
            }

            await this._serviceBase.UpdateList(talleres);
        }

        public async Task UpdateRutasPorGalpon(GalponDto galpon )
        { 
            foreach (var item in galpon.Rutas)
            {
                if (item.BanderaId != 0)
                {
                    //await rutasservice.UpdateAsync(item);
                    var bandEntity = await this.banderaservice.GetByIdAsync(item.BanderaId);
                    if (bandEntity.DesBan != item.BanderaNombre) {
                        bandEntity.DesBan = item.BanderaNombre;
                        await this.banderaservice.UpdateAsync(bandEntity);
                    }

                }
                else
                {

                    //var nombre =  string.Format("{0} {1}", item.Puntos.First(e => e.EsPuntoInicio).Abreviacion.Trim(), item.Puntos.First(e => e.EsPuntoTermino).Abreviacion.Trim()).Truncate(15);
                    var nombre = item.BanderaNombre;

                    var bandera = new HBanderas
                    {
                        DesBan = nombre,
                        SenBan = nombre.Truncate(10),
                        SucursalId = item.SucursalId,
                        TipoBanderaId = PlaTipoBandera.Posicionamiento,
                        Activo = true,
                        CodigoVarianteLinea = item.CodigoVarianteLinea,
                        AbrBan = nombre.Truncate(4),
                    };

                    await banderaservice.AddAsync(bandera);
                    item.BanderaId = bandera.Id;

                    item.FechaVigenciaDesde = DateTime.Now.Date;
                    item.EstadoRutaId = PlaEstadoRuta.APROBADO;
                    item.Nombre = nombre;

                    await rutasservice.AddAsync(item);
                }
            }


            foreach (var item in galpon.BanderasAEliminar)
            {
                await banderaservice.DeleteAsync(item);
            }
        }
    }
}
