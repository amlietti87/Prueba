using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinTipoLesionado : FullAuditedEntity<int>
    {

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        [NotMapped]
        public ICollection<SinLesionados> SinLesionados { get; set; }
    }
}
