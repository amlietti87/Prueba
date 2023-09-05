using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaPuntos : Entity<Guid>
    {
        public PlaPuntos()
        {            
            SectorPuntoFin = new HashSet<PlaSector>();
            SectorPuntoInicio = new HashSet<PlaSector>();
        }

       // [NotMapped]
        //public override int Id { get => base.Id; set => base.Id = value; }

        public int CodRec { get; set; }

        public decimal Lat { get; set; }
        public decimal Long { get; set; }

        public string CodigoNombre { get; set; }
        public string Data { get; set; }
        
        public bool EsPuntoInicio { get; set; }
        public bool EsPuntoTermino { get; set; }
        public bool EsParada { get; set; }
        public bool EsCambioSector { get; set; }

        public bool EsPuntoRelevo { get; set; }
        public bool EsCambioSectorTarifario { get; set; }

        public int? CodSectorTarifario { get; set; }
        public BolSectoresTarifarios BolSectoresTarifarios { get; set; }

        public int Orden { get; set; }
        public int? TipoParadaId { get; set; }
 
        public string Abreviacion { get; set; }
        public bool? MostrarEnReporte { get; set; }
        public string Color { get; set; }


        public int? PlaCoordenadaId { get; set; }
        public int? PlaParadaId { get; set; }
        public PlaCoordenadas PlaCoordenada { get; set; }

        public GpsRecorridos Ruta { get; set; }        
        public PlaTipoParada TipoParada { get; set; }
        public virtual PlaParadas PlaParada { get; set; }
        public ICollection<PlaSector> SectorPuntoFin { get; set; }       
        public ICollection<PlaSector> SectorPuntoInicio { get; set; }  
        public int? PlaTipoViajeId { get; set; }
        public PlaTipoViaje PlaTipoViaje { get; set; }
        

    }
}
