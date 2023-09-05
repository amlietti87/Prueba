using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class RamalSube : Entity<long>
    {
       
        public long RamalColorId { get; set; }
        public int EmpresaId { get; set; }
        public string CodigoSube { get; set; }
        public Empresa Empresa { get; set; }
        public PlaRamalColor RamalColor { get; set; }

    }
}
