using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaSentidoBanderaDto :EntityDto<int>
    {
        [StringLength(100)]
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public override string Description => this.Descripcion;
    }
}
