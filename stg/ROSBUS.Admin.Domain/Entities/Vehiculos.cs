using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinVehiculos : Entity<int>
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Dominio { get; set; }
        public int? SeguroId { get; set; }
        public string Poliza { get; set; }

        public SinCiaSeguros Seguro { get; set; }
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }
    }

    public partial class SinVehiculos : Entity<int> {

        public string GetDescription() {
            return string.Format("{0} {1} {2}", this.Marca , this.Modelo, this.Dominio);
        }
    }

}
