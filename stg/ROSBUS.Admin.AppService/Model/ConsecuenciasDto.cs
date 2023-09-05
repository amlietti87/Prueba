using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ConsecuenciasDto :EntityDto<int>
    {
        public ConsecuenciasDto()
        {
            this.Categorias = new List<CategoriasDto>();
        }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public bool Adicional { get; set; }

        [Required]
        public bool Anulado { get; set; }

        [Required]
        public bool Responsable { get; set; }
        public int? CategoriaElegida { get; set; }

        public string CategoriaNombre { get; set; }

        public string Observaciones { get; set; }

        public override string Description => this.Descripcion;

        public List<CategoriasDto> Categorias { get; set; }
    }
}
