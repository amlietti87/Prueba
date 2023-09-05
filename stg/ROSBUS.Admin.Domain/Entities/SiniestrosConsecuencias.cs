using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSiniestrosConsecuencias : FullAuditedEntity<int>
    {
        public int SiniestroId { get; set; }
        public int ConsecuenciaId { get; set; }
        public string Observaciones { get; set; }
        public int? CategoriaId { get; set; }

        public SinCategorias Categoria { get; set; }
        public SinConsecuencias Consecuencia { get; set; }
        public SinSiniestros Siniestro { get; set; }

        public int? Cantidad { get; set; } 
    }
}
