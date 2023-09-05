
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtMotivosNotificaciones : AuditedEntity<int>
    {
        public ArtMotivosNotificaciones()
        {
            ArtDenunciasNotificaciones = new HashSet<ArtDenunciasNotificaciones>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public ICollection<ArtDenunciasNotificaciones> ArtDenunciasNotificaciones { get; set; }

    }
}
