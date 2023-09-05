using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspDiagramasInspectores : FullAuditedEntity<int>, IConcurrencyEntity
    {
        public InspDiagramasInspectores()
        {
            InspDiagramaInspectoresTurnos = new HashSet<InspDiagramasInspectoresTurnos>();
        }

        public int Mes { get; set; }
        public int Anio { get; set; }
        public long GrupoInspectoresId { get; set; }
        public int EstadoDiagramaId { get; set; }

        public InspEstadosDiagramaInspectores EstadoDiagrama { get; set; }
        public InspGruposInspectores GrupoInspectores { get; set; }
        public ICollection<InspDiagramasInspectoresTurnos> InspDiagramaInspectoresTurnos { get; set; }

        [NotMapped]
        public DateTime? BlockDate { get; set; }

        public string GetEnityId()
        {
            return this.Id.ToString();
        }
    }


   

}
