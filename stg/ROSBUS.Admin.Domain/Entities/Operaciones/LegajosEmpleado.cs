using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class LegajosEmpleado : Entity<int>
    {
        //public int CodEmpleado { get; set; }
        public DateTime FecIngreso { get; set; }
        public string LegajoSap { get; set; }
        public DateTime? FecBaja { get; set; }
        public int? CodEmpresa { get; set; }
        public DateTime? FecProcesado { get; set; }
        public Empleados Empleado { get; set; }



    }
}
