using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdDocumentosProcesadosHistorico : CreationAuditedEntity<long>
    {

        public FdDocumentosProcesadosHistorico()
        {

        }
        public long DocumentoProcesadoId { get; set; }
        public Guid ArchivoId { get; set; }
        public int EstadoId { get; set; }
        //public string CodMinisterio { get; set; }
        public bool Rechazado { get; set; }

        public string NombreArchivo { get; set; }
        public FdDocumentosProcesados DocumentoProcesado { get; set; }
        public FdEstados Estado { get; set; }

        public SysUsersAd Usuario { get; set; }

    }
}
