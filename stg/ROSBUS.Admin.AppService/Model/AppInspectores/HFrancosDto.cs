using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class HFrancosDto : AuditedEntityDto<int>
    {
        public DateTime Fecha { get; set; }
        public int CodNov { get; set; }
        public string CodUsu { get; set; }
        public string Modificable { get; set; }
        public string Observacion { get; set; }
        public string PasadoSueldos { get; set; }
        public int? TurnoId { get; set; }
        public int? DiagramaInspectoresId { get; set; }
        public int? JornadasTrabajadaId { get; set; }
        public override string Description => this.CodUsu;

       // public PersJornadasTrabajadasDto JornadasTrabajada { get; set; }

        
    }
}
