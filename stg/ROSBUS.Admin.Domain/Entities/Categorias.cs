using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinCategorias : FullAuditedEntity<int>
    {
        public int ConsecuenciaId { get; set; }
        public string Descripcion { get; set; }
        public string InfoAdicional { get; set; }
        public bool Anulado { get; set; }

        public SinConsecuencias Consecuencia { get; set; }
        public ICollection<SinSiniestrosConsecuencias> SinSiniestrosConsecuencias { get; set; }
    }
}
