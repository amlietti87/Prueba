using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtMotivosNotificacionesDto : EntityDto<int>
    {
        public ArtMotivosNotificacionesDto()
        {
            this.ArtDenunciasNotificacionesDto = new List<ArtDenunciasNotificacionesDto>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public List<ArtDenunciasNotificacionesDto> ArtDenunciasNotificacionesDto { get; set; }

        public override string Description => Descripcion;
    }
}
