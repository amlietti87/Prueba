using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinConsecuencias : FullAuditedEntity<int>
    {

        public SinConsecuencias()
        {
            SinCategorias = new HashSet<SinCategorias>();
            SinSiniestrosConsecuencias = new HashSet<SinSiniestrosConsecuencias>();
        }

        public string Descripcion { get; set; }
        public bool Adicional { get; set; }
        public bool Anulado { get; set; }

        public bool Responsable { get; set; }

        public ICollection<SinCategorias> SinCategorias { get; set; }
        public ICollection<SinSiniestrosConsecuencias> SinSiniestrosConsecuencias { get; set; }


        
    }
}
