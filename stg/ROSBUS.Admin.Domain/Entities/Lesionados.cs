using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinLesionados : Entity<int>
    {

        public int? TipoLesionadoId { get; set; }

        [NotMapped]
        public SinTipoLesionado TipoLesionado { get; set; }
        [NotMapped]
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }

        public string GetDescription()
        {
            return string.Format("{0}", this.TipoLesionado?.Descripcion );
        }
    }
}
