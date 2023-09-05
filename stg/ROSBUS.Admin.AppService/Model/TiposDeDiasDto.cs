using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class TiposDeDiasDto :EntityDto<int>
    {  
        public override string Description => this.DesTdia;

        public bool Activo { get; set; }

        public string  Color { get; set; }

        public string DesTdia { get; set; }
        public int? CopiaTipoDiaId { get; set; }  
        public string Descripcion { get; set; }
        public int Orden { get; set; }
    }
}
