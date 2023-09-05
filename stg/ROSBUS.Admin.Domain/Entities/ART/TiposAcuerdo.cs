
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.ART.Domain.Entities.ART
{
    public partial class TiposAcuerdo : AuditedEntity<int>
    {
        public TiposAcuerdo()
        {
            Reclamos = new HashSet<SinReclamos>();
            ReclamosHistoricos = new HashSet<SinReclamosHistoricos>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinReclamos> Reclamos { get; set; }
        public ICollection<SinReclamosHistoricos> ReclamosHistoricos { get; set; }
    }
}
