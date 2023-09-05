using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinTipoMueble : FullAuditedEntity<int>
    {

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinMuebleInmueble> SinMuebleInmueble { get; set; }
    }
}
