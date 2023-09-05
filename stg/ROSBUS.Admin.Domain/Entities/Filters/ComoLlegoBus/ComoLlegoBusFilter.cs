using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus
{
    public class ComoLlegoBusFilter
    {
        public string Origen { get; set; }
        public string Destino { get; set; }
        public int CantidadCuadras { get; set; }
        public Guid? ParentRouteId { get; set; }
        public Guid? GrupoRoutesId { get; set; }
        public int? CodBan { get; set; }
        public int? CodLin { get; set; }
        public DateTime? Fecha { get; set; }

    }
}
