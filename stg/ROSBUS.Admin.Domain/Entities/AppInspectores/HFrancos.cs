using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public  class HFrancos : AuditedEntity<string>
    {
        
        public DateTime Fecha { get; set; }
        public int CodNov { get; set; }
        public string CodUsu { get; set; }
        public string Modificable { get; set; }
        public string Observacion { get; set; }
        public string PasadoSueldos { get; set; }
        public int? RangoHorarioId { get; set; }
        public int? DiagramaInspectoresTurnoId { get; set; }
        public int? JornadasTrabajadaId { get; set; }

        public InspDiagramasInspectoresTurnos DiagramaInspectoresTurno { get; set; }
        public PersJornadasTrabajadas JornadasTrabajada { get; set; }
        public InspRangosHorarios RangoHorario { get; set; }
    }
}
