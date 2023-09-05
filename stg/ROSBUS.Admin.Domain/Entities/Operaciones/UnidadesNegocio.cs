using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class UnidadesNegocio : Entity<string>
    {
        public string descripcion { get; set; }
        public int? cod_sucursal { get; set; }
        public ICollection<Empleados> Empleados { get; set; }

        public UnidadesNegocio()
        {
            Empleados = new HashSet<Empleados>();
        }

    }
}
