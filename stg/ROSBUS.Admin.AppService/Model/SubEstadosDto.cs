using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SubEstadosDto :EntityDto<int>
    {
        [Required]
        public string Descripcion { get; set; }

        [Required]
        public bool Cierre { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public bool Anulado { get; set; }

        public string EstadoNombre { get; set; }

        public override string Description => this.Descripcion;

    }
}
