
using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenunciaAdjuntos
    {
        public int DenunciaId { get; set; }
        public Guid AdjuntoId { get; set; }

        public ArtDenuncias Denuncia { get; set; }
    }
}
