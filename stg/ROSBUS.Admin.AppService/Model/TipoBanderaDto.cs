using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class TipoBanderaDto :EntityDto<int>
    {
        [StringLength(150)]
        public string Nombre { get; set; }

        public override string Description => this.Nombre;
    }
}
