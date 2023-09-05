using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinInvolucradosAdjuntos 
    {
        public int InvolucradoId { get; set; }
        public Guid AdjuntoId { get; set; }

        //public Adjuntos Adjunto { get; set; }
        public SinInvolucrados Involucrado { get; set; }
    }
}
