using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdDocumentosProcesadosHistoricoDto :EntityDto<long>
    {

        public long DocumentoProcesadoId { get; set; }
        public Guid ArchivoId { get; set; }
        public int EstadoId { get; set; }
        //public string CodMinisterio { get; set; }
        public bool Rechazado { get; set; }

        public FdEstadosDto Estado { get; set; }
        public override string Description => String.Empty;
    }
}
