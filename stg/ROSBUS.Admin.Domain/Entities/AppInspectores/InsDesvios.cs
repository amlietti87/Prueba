using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InsDesvios : FullAuditedEntity<long>
    {
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public int SucursalId { get; set; }
        public bool Leido { get; set; }
    }
}
