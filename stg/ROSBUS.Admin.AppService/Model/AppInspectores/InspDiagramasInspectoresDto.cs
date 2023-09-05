using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspDiagramasInspectoresDto : FullAuditedEntityDto<int>
    {
        public InspDiagramasInspectoresDto()
        {
            HFrancos = new List<HFrancosDto>();
            PersJornadasTrabajadas = new List<PersJornadasTrabajadasDto>();
            InspDiagramaInspectoresTurnos = new List<InspDiagramasInspectoresTurnosDto>();
        }

        public int Mes { get; set; }
        public int Anio { get; set; }
        public long GrupoInspectoresId { get; set; }
        public int EstadoDiagramaId { get; set; }
        public string GrupoNombre { get; set; }
        public string EstadoNombre { get; set; }
        public InspGruposInspectoresDto GrupoInspectores { get; set; }
        public InspEstadosDiagramaInspectoresDto EstadoDiagrama { get; set; }
        public List<HFrancosDto> HFrancos { get; set; }
        public List<PersJornadasTrabajadasDto> PersJornadasTrabajadas { get; set; }
        public List<InspDiagramasInspectoresTurnosDto> InspDiagramaInspectoresTurnos { get; set; }

        public override string Description => this.Mes.ToString() + "-" + this.Anio.ToString();
    }
}
