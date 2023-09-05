using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class CroElemenetoDto :EntityDto<Guid>
    {
        [StringLength(100)]
        public string Nombre { get; set; }

        public override string Description => this.Nombre;


        public int? TipoId { get; set; }
        public int? TipoElementoId { get; set; } 
        public string Descripcion { get; set; }


//        public CroTipoDto Tipo { get; set; }
       // public  CroTipoElementoDto TipoElemento { get; set; }

         public String DescripcionTipoElemento { get; set; }

    }
}
