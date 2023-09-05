using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaCodigoSubeBandera : Entity<long>
    {
        public int CodBan { get; set; }
        public decimal CodEmpresa { get; set; }
        public long CodigoSube { get; set; }
        public string Descripcion { get; set; }

        public int? SentidoBanderaSubeId { get; set; }

        public HBanderas CodBanNavigation { get; set; }

        public PlaSentidoBanderaSube PlaSentidoBanderaSubeNavigation { get; set; }
        public Empresa CodEmpresaNavigation { get; set; }
    }
}
