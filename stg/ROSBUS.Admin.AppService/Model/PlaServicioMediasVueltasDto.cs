using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaServicioMediasVueltasDto :EntityDto<int>
    {
        [StringLength(100)]
        public string Nombre { get; set; }

        public override string Description => this.Nombre;
    }
}
