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
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class SectorAppService : AppServiceBase<PlaSector, SectorDto, Int64, ISectorService>, ISectorAppService
    {
        private readonly IPuntosService puntoService;
        private readonly IRutasService rutaservice;

        public SectorAppService(ISectorService serviceBase, IPuntosService _puntos, IRutasService _rutaservice) 
            :base(serviceBase)
        {
            this.puntoService = _puntos;
            this.rutaservice = _rutaservice;
        }

        

        public async Task<RutaSectoresDto> GetSectorView(SectorConPuntosFilter filter)
        {

            var result = new  RutaSectoresDto();

            if (filter.RutaId.HasValue)
            {
                var ruta = await this.rutaservice.GetByIdAsync(filter.RutaId.Value);
                result.Nombre = ruta.Nombre;
                result.BanderaId = ruta.CodBan;
                result.Id = ruta.Id;
            }
    


            var sectores = (await this._serviceBase.GetPagedListAsync(filter)).Items;

          
            sectores = sectores.OrderBy(e => e.PuntoInicio.Orden).ToList();

            var puntosFilter = new PuntosFilter() { RutaId = filter.RutaId };
            
            var puntos = (await this.puntoService.GetAllAsync(puntosFilter.GetFilterExpression())).Items;

            if (!puntos.Any())
            {
                throw new ValidationException("No existen puntos");
            }

            var value = 200;

            result.Sectores = new List<SectorViewDto>();

            var first = puntos.Where(e => e.EsPuntoInicio).FirstOrDefault();
            result.Sectores.Add(new SectorViewDto(value += 2, first.Abreviacion ?? first.CodigoNombre, first.PlaCoordenada.NumeroExternoIVU, true));

            foreach (var sector in sectores)
            {
                
                var sectorview = new SectorViewDto(value + 1, sector.Descripcion, string.Format("KM: {0}", sector.DistanciaKm.GetValueOrDefault().ToString("0.##")), false);
                this.AddItemSector(puntos, sector, sectorview);

                result.Sectores.Add(sectorview);

               
                var puntoparada = puntos.Single(e => e.Id == sector.PuntoFinId);
                var parada = new SectorViewDto(value += 2, puntoparada.Abreviacion ?? first.CodigoNombre, puntoparada.PlaCoordenada.NumeroExternoIVU, true);
                result.Sectores.Add(parada);
            }

            return result;    

        }

        private void AddItemSector(IReadOnlyList<PlaPuntos> puntos, PlaSector sector, SectorViewDto sectorview)
        {
            var paradeasdelsector = BuscarParadasSectorDelSector(sector, puntos);
            var p = 0;
            foreach (var punto in paradeasdelsector)
            {
                sectorview.Items.Add(new ItemSectorViewDto(p += 1, punto.Abreviacion, string.Format("Codigo: {0} - Numero Externo: {1}", punto.CodigoNombre, punto.PlaCoordenada.NumeroExternoIVU)));
            }
        }

        private List<PlaPuntos> BuscarParadasSectorDelSector(PlaSector sector, IEnumerable<PlaPuntos> Puntos)
        {


            var pi = Puntos.Single(e => e.Id == sector.PuntoInicioId);
            var pf = Puntos.Single(e => e.Id == sector.PuntoFinId);

            return Puntos.Where(s => s.EsParada && s.Orden > pi.Orden && s.Orden <= pf.Orden).ToList();
        }

        public async Task<List<PlaSentidoPorSector>> RecuperarSentidoPorSector(HDesignarFilter Filtro)
        {
            var sectores = await _serviceBase.RecuperarSentidoPorSector(Filtro);

            return sectores;
        }



    }
}
