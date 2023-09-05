using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class TiempoEsperadoDeCargaDto :EntityDto<Int64>
    {    

        public override string Description => "";

        public int? TipodeDiaId { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public TimeSpan TiempoDeCarga { get; set; }
        public int TipoParadaId { get; set; }
        public String TipoDiaNombre { get; set; }
    }
}
