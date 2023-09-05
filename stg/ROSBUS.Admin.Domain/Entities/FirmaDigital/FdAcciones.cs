using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdAcciones : Entity<int>
    {
        public int TipoDocumentoId { get; set; }
        public int AccionPermitidaId { get; set; }
        public int EstadoActualId { get; set; }
        public int EstadoNuevoId { get; set; }
        public bool MostrarBdempleador { get; set; }
        public bool MostrarBdempleado { get; set; }
        public bool AccionBdempleador { get; set; }
        public bool EsFin { get; set; }
        public bool GeneraNotificacion { get; set; }
        public int? NotificacionId { get; set; }

        public Boolean PermiteMarcarLote { get; set; }
        public Boolean MenorFechaPrimero { get; set; }
        //public Boolean GeneraNotificacionFirmador { get; set; }


        public FdAccionesPermitidas AccionPermitida { get; set; }
        public FdEstados EstadoActual { get; set; }
        public FdEstados EstadoNuevo { get; set; }
        public FdTiposDocumentos TipoDocumento { get; set; }
    }
}
