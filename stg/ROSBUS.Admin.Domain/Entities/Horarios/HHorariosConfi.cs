using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HHorariosConfi : TECSO.FWK.Domain.Entities.Entity<int>
    {
        public HHorariosConfi()
        {
            HServicios = new HashSet<HServicios>();
        }

        public int CodHfecha { get; set; }
        public int CodTdia { get; set; }
        public decimal CodSubg { get; set; }
      //  public int CodHconfi { get; set; }
        public decimal? CantCoches { get; set; }
        public int? CantConductores { get; set; }
        public int? CantidadCochesReal { get; set; }
        public int? CantidadConductoresReal { get; set; }

        public int? CodHconfibsas { get; set; }

        public HFechasConfi CodHfechaNavigation { get; set; }
        public SubGalpon CodSubgNavigation { get; set; }
        public HTipodia CodTdiaNavigation { get; set; }
        public ICollection<HServicios> HServicios { get; set; }
    }
}
