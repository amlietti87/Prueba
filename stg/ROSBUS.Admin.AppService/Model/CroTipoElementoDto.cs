using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;

namespace ROSBUS.Admin.AppService.Model
{
    public class CroTipoElementoDto : TECSO.FWK.Domain.Entities.EntityDto<int>
    {

        public CroTipoElementoDto()
        {
            CroElemeneto = new HashSet<CroElemenetoDto>();
        }

        [StringLength(100)]
        public string Nombre { get; set; }

        public override string Description => this.Nombre;

        public ICollection<CroElemenetoDto> CroElemeneto { get; set; }
    }
}
