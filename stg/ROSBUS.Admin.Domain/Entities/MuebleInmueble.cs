using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinMuebleInmueble : Entity<int>
    {
        public int TipoInmuebleId { get; set; }
        public string Lugar { get; set; }
        public int? LocalidadId { get; set; }

        public SinTipoMueble TipoInmueble { get; set; }
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }

        
    }
}
