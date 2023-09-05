using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class LegajosEmpleadoDto :EntityDto<int>
    {
        //public int CodEmpleado { get; set; }
        public DateTime FecIngreso { get; set; }
        public string LegajoSap { get; set; }
        public DateTime? FecBaja { get; set; }
        public int? CodEmpresa { get; set; }
        public DateTime? FecProcesado { get; set; }

        public override string Description => string.Empty;
    }
}
