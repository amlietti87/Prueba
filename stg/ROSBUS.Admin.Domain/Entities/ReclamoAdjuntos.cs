using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinReclamoAdjuntos  
    {
        public int ReclamoId { get; set; }
        public Guid AdjuntoId { get; set; }

        //public Adjuntos Adjunto { get; set; }
        public SinReclamos Reclamo { get; set; }
    }
}
