using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class FdFirmadorDetalle : AuditedEntity<long>
    {
        public long FirmadorId { get; set; }
        public long DocumentoProcesadoId { get; set; }
        public bool Firmado { get; set; }
        public Guid? ArchivoIdRecibido { get; set; }
        public int EstadoId { get; set; }
        public Guid ArchivoIdEnviado { get; set; }

        public virtual FdDocumentosProcesados DocumentoProcesado { get; set; }


        public virtual FdFirmador Firmador { get; set; }
        
    }
}
