using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinTipoInvolucrado : FullAuditedEntity<int>
    {

        public string Descripcion { get; set; }
        public bool Reclamo { get; set; }
        public bool Conductor { get; set; }
        public bool Vehiculo { get; set; }
        public bool MuebleInmueble { get; set; }
        public bool Lesionado { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }
    }
}
