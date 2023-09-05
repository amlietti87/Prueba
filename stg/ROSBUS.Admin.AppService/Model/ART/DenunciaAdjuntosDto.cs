using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenunciaAdjuntosDto : EntityDto<long>
    {
        public int DenunciaId { get; set; }
        public Guid AdjuntoId { get; set; }
        public override string Description => string.Empty;
        //public ArtDenunciasDto DenunciaDto { get; set; }
    }
}
