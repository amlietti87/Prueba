using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class CroCroquisDto :EntityDto<int>
    {
        [NotMapped]
        public string Nombre { get; set; }

        public string Svg { get; set; }
        public int? TipoId { get; set; }
        [NotMapped]
        public int? SiniestroId { get; set; }
        public override string Description => this.Nombre;
    }
}
