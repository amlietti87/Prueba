using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdTiposDocumentos : AuditedEntity<int>
    {
        public FdTiposDocumentos()
        {
            FdAcciones = new HashSet<FdAcciones>();
            FdDocumentosError = new HashSet<FdDocumentosError>();
            FdDocumentosProcesados = new HashSet<FdDocumentosProcesados>();
        }

        
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public bool RequiereLider { get; set; }
        public bool EsPredeterminado { get; set; }
        public bool Anulado { get; set; }
        
        public ICollection<FdAcciones> FdAcciones { get; set; }
        public ICollection<FdDocumentosError> FdDocumentosError { get; set; }
        public ICollection<FdDocumentosProcesados> FdDocumentosProcesados { get; set; }
    }

}
