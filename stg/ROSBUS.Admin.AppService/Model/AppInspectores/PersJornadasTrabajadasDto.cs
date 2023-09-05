using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class PersJornadasTrabajadasDto : FullAuditedEntityDto<int>
    {
        public PersJornadasTrabajadasDto()
        {
            HFrancos = new List<HFrancosDto>();
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
        public int? TurnoId { get; set; }
        public int? DiagramaInspectoresId { get; set; }
        public int? ZonaId { get; set; }

        public override string Description => this.CodEmpleado;


        public List<HFrancosDto> HFrancos { get; set; }

    }
}
