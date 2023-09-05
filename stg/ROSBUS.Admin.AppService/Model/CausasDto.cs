using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class CausasDto :EntityDto<int>
    {
        public CausasDto()
        {
            this.SubCausas = new List<SubCausasDto>();
        }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public bool Anulado { get; set; }
        [Required]
        public bool Responsable { get; set; }

        public override string Description => this.Descripcion;

        public List<SubCausasDto> SubCausas { get; set; }

    }
}
