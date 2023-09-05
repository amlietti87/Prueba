using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HTipodia : FullAuditedEntity<int>
    {
        public HTipodia()
        {
            PlaTiempoEsperadoDeCarga = new HashSet<PlaTiempoEsperadoDeCarga>();
            HHorariosConfi = new HashSet<HHorariosConfi>();
            HMinxtipo = new HashSet<HMinxtipo>();
            PlaDistribucionDeCochesPorTipoDeDia = new HashSet<PlaDistribucionDeCochesPorTipoDeDia>();

        }

        public bool Activo { get; set; }

        public string Color { get; set; }

        public string DesTdia { get; set; }

        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public ICollection<PlaTiempoEsperadoDeCarga> PlaTiempoEsperadoDeCarga { get; set; }

        public ICollection<HHorariosConfi> HHorariosConfi { get; set; }
        public ICollection<HMinxtipo> HMinxtipo { get; set; }

        public ICollection<PlaDistribucionDeCochesPorTipoDeDia> PlaDistribucionDeCochesPorTipoDeDia { get; set; }

    }


    public partial class HTipodia  
    {

        [NotMapped]
        public int? CopiaTipoDiaId { get; set; }
    }
}
