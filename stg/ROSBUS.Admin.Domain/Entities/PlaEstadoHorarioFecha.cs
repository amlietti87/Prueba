using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaEstadoHorarioFecha : Entity<int>
    {
        public PlaEstadoHorarioFecha()
        {
            HFechasConfi = new HashSet<HFechasConfi>();
        }

       // public int Id { get; set; }
        public string Descripcion { get; set; }

        public ICollection<HFechasConfi> HFechasConfi { get; set; }



    }

    public partial class PlaEstadoHorarioFecha
    {
        public const int Borrador = 1;
        public const int Aprobado = 2;
    }


}
