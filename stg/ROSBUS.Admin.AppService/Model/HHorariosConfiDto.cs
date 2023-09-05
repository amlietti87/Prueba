using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HHorariosConfiDto :EntityDto<int>
    {
        public int CodHfecha { get; set; }
        public int CodTdia { get; set; }
        public decimal CodSubg { get; set; }
        //  public int CodHconfi { get; set; }
        public decimal? CantCoches { get; set; }
        public int? CantConductores { get; set; }
        public int? CodHconfibsas { get; set; }
        public int? CantidadCochesReal { get; set; } 
        public int? CantidadConductoresReal { get; set; }

        public override string Description => null;

        public HServiciosDto CurrentServicio { get; set; }
    }
}
