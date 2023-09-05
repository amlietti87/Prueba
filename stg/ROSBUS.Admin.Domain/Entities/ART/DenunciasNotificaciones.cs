
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenunciasNotificaciones:FullAuditedEntity<int>
    {
        public int DenunciaId { get; set; }
        public int MotivoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }

        public ArtDenuncias Denuncia { get; set; }
        public ArtMotivosNotificaciones Motivo { get; set; }

    }
}
