using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtDenunciasNotificacionesDto : EntityDto<int>
    {
        [Required]
        public int DenunciaId { get; set; }
        [Required]
        public int MotivoId { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }

        public ArtDenunciasDto DenunciaDto { get; set; }
        public ArtMotivosNotificacionesDto MotivoDto { get; set; }

        public override string Description => string.Empty;
    }
}
