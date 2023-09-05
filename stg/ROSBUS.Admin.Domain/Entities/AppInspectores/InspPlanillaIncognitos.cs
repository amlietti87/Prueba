using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
namespace ROSBUS.Admin.Domain.Entities
{
    public class InspPlanillaIncognitos : FullAuditedEntity<int>
    {
        public InspPlanillaIncognitos()
        {
            InspPlanillaIncognitosDetalle = new HashSet<InspPlanillaIncognitosDetalle>();
        }
        public int? SucursalId { get; set; } 
        public DateTime Fecha { get; set; }
        public DateTime HoraAscenso { get; set; }
        public DateTime? HoraDescenso { get; set; }
        public string CocheId { get; set; }
        public int CocheFicha { get; set; }
        public string CocheInterno { get; set; }
        public decimal Tarifa { get; set; }
        public virtual ICollection<InspPlanillaIncognitosDetalle> InspPlanillaIncognitosDetalle { get; set; }
        
    }
}
