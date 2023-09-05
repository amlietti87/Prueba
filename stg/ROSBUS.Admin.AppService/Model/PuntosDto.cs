using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PuntosDto :EntityDto<Guid>
    { 
        public override string Description => this.CodigoNombre;         

        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string CodigoNombre { get; set; }
        public string Data { get; set; }
        public long RutaId { get; set; }
        public bool EsPuntoInicio { get; set; }
        public bool EsPuntoTermino { get; set; }
        public bool EsParada { get; set; }
        public bool EsCambioSector { get; set; }
        public bool EsPuntoRelevo { get; set; }
        public bool EsCambioSectorTarifario { get; set; }
        public int Orden { get; set; }
        public int? TipoParadaId { get; set; }        
        public string NumeroExterno { get; set; }
        public bool MostrarEnReporte { get; set; }
        public string Abreviacion { get; set; }
        public string Color { get; set; }

        public Guid? IdentificadorMapa { get; set; }

        public string CodigoVarianteLinea { get; set; }

        public int? PlaCoordenadaId { get; set; }
        public int? PlaParadaId { get; set; }
        public int? CodSectorTarifario { get; set; }

        public ItemDto<int> PlaCoordenadaItem { get; set; }

        public bool? PlaCoordenadaAnulada { get; set; }
        public ItemDto<int> SectoresTarifariosItem { get; set; }

        public ItemDto<int> PlaParadaItem { get; set; }

        public string PlaParadaCruceCalle { get; set; }

        public string PlaCoordenadaCalle1 { get; set; }
        public string PlaCoordenadaCalle2 { get; set; }

        
        public int? PlaTipoViajeId { get; set; }

        public bool? PickUpType { get; set; }
        public bool? DropOffType { get; set; }

    }
}
