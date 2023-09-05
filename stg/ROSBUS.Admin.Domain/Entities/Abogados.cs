using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinAbogados : FullAuditedEntity<int>
    {
        public string ApellidoNombre { get; set; }
        public string Domicilio { get; set; }
        public int? LocalidadId { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        public bool Anulado { get; set; }
        public ICollection<SinReclamos> SinReclamosAbogado { get; set; }
        public ICollection<SinReclamos> SinReclamosAbogadoEmpresa { get; set; }

        public ICollection<SinReclamosHistoricos> SinReclamosHistoricoAbogado { get; set; }
        public ICollection<SinReclamosHistoricos> SinReclamosHistoricoAbogadoEmpresa { get; set; }
    }
}
