using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class GalponDto : EntityDto<Decimal>
    {
        public GalponDto()
        {
            this.Rutas = new List<RutasDto>();
            this.BanderasAEliminar = new List<int>();
        }


        public override string Description => this.Nombre;

        [StringLength(25)]
        [Required]
        public string Nombre { get; set; }
        

        public decimal Lat { get; set; }
        public decimal Long { get; set; }

        [Required]
        [StringLength(4)]
        public string PosGal { get; set; }

        public List<RutasDto> Rutas { get; set; }
        public List<int> BanderasAEliminar { get; set; }

    }
}
