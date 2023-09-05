using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class TipoLineaDto : EntityDto<int>
    {
        [StringLength(100)]
        public string Nombre { get; set; } 
        public bool Activo { get; set; }

        public int CantidadConductoresPorServicio { get; set; }

        public override string Description => this.Nombre;
    }
}
