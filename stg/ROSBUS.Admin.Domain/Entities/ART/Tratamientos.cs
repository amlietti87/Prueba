
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtTratamientos : AuditedEntity<int>
    {
        public ArtTratamientos()
        {
            ArtDenuncias = new HashSet<ArtDenuncias>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public ICollection<ArtDenuncias> ArtDenuncias { get; set; }


    }
}
