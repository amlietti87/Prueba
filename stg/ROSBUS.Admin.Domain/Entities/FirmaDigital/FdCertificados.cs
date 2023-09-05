using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{

    public partial class FdCertificados : AuditedEntity<int, SysUsersAd>
    {
        public int UsuarioId { get; set; }
        public Guid ArchivoId { get; set; }
        public DateTime FechaActivacion { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRevocacion { get; set; }

        public SysUsersAd Usuario { get; set; }
    }
}
