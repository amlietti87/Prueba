using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class TipoDni : Entity<int>
    {

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }
        public ICollection<SinPracticantes> SinPracticantes { get; set; }
        public ICollection<SinConductores> SinConductores { get; set; }

    }
}
