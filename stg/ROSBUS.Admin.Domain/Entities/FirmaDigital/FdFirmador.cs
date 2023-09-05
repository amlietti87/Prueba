using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class FdFirmador : CreationAuditedEntity<long>
    {
        public FdFirmador()
        {
            FdFirmadorDetalle = new HashSet<FdFirmadorDetalle>();
            FdFirmadorLog = new HashSet<FdFirmadorLog>();
        }

        public string SessionId { get; set; }
        public string PathGetDescarga { get; set; }
        public string PathPostSubida { get; set; }

        public int AccionId { get; set; }

        public string UsuarioId { get; set; }
        public string UsuarioRol { get; set; }
        public string UsuarioUserName { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string CoordenadasEmpleado { get; set; }
        public string CoordenadasEmpleador { get; set; }

        public virtual ICollection<FdFirmadorDetalle> FdFirmadorDetalle { get; set; }
        public virtual ICollection<FdFirmadorLog> FdFirmadorLog { get; set; }
    }
}
