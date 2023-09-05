using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinSiniestroAdjuntos 
    {
        public int SiniestroId { get; set; }

        public Guid AdjuntoId { get; set; }
        
        public SinSiniestros Siniestro { get; set; }
    }
}
