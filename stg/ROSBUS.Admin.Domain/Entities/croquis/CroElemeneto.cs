using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class CroElemeneto : TECSO.FWK.Domain.Entities.Entity<Guid>
    {
       // public new Guid Id { get; set; }
        public int? TipoId { get; set; }
        public int? TipoElementoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public CroTipo Tipo { get; set; }
        public CroTipoElemento TipoElemento { get; set; }
    }
}
