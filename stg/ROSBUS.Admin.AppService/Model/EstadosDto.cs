using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class EstadosDto :EntityDto<int>
    {

        public EstadosDto()
        {
            this.SubEstados = new List<SubEstadosDto>();
        }

        [Required]
        public string Descripcion { get; set; }
        public int? OrdenCambioEstado { get; set; }

        [Required]
        public bool Judicial { get; set; }

        [Required]
        public bool Anulado { get; set; }


        public override string Description => this.Descripcion;


        public List<SubEstadosDto> SubEstados { get; set; }

    }
}
