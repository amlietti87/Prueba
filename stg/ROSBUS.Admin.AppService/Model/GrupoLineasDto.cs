using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class GrupoLineasDto : EntityDto<int>
    {
        public GrupoLineasDto()
        {
            this.Lineas = new List<ItemDecimalDto>();
        }

        public string Nombre { get; set; }


        [Required]
        public int SucursalId { get; set; }

        public override string Description => this.Nombre;

        public string Sucursal { get; set; }
        public long LineasTotales { get; set; }

        public List<ItemDecimalDto> Lineas { get; set; }



    }
}
