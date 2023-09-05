using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTiempoEsperadoDeCarga : Entity<long>
    {
        
        public int? TipodeDiaId { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public TimeSpan TiempoDeCarga { get; set; }
        public int TipoParadaId { get; set; }

        public PlaTipoParada TipoParada { get; set; }
        public HTipodia TipodeDia { get; set; }
    }
}
