using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaSector : Entity<long>
    {


        public string Descripcion { get; set; }
        public string Data { get; set; }
        public decimal? DistanciaKm { get; set; }
        public Guid PuntoInicioId { get; set; }
        public Guid PuntoFinId { get; set; }
        public int CodRec { get; set; }
        public string Color { get; set; }


        public PlaPuntos PuntoFin { get; set; }
        public PlaPuntos PuntoInicio { get; set; }

        public GpsRecorridos Ruta { get; set; }
        public ICollection<PlaMinutosPorSector> PlaMinutosPorSector { get; set; }



    }

    public class PlaSentidoPorSector
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
}
