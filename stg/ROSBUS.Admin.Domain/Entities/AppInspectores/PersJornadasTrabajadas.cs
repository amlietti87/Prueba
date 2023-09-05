using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class PersJornadasTrabajadas : FullAuditedEntity<int>
    {
        public PersJornadasTrabajadas()
        {
            HFrancos = new HashSet<HFrancos>();
        }

        public int CodGalpon { get; set; }
        public int CodArea { get; set; }
        public int CodTurno { get; set; }
        public string CodEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? HoraDesde { get; set; }
        public DateTime HoraDesdeModif { get; set; }
        public DateTime? HoraHasta { get; set; }
        public DateTime HoraHastaModif { get; set; }
        public string TpoServicio { get; set; }
        public DateTime? HorasDescanso { get; set; }
        public string PasadaSueldos { get; set; }
        public string Duracion { get; set; }
        public int? CodJornadaTrabajadabsas { get; set; }
        public int? RangoHorarioId { get; set; }
        public int? DiagramaInspectoresTurnoId { get; set; }
        public int? ZonaId { get; set; }
        public bool? Pago { get; set; }

        public InspDiagramasInspectoresTurnos DiagramaInspectoresTurno { get; set; }
        public InspRangosHorarios RangoHorario { get; set; }
        public InspZonas Zona { get; set; }

        public ICollection<HFrancos> HFrancos { get; set; }

    }
}
