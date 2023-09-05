using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class TipoInvolucradoDto :EntityDto<int>
    {
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public bool Reclamo { get; set; }
        [Required]
        public bool Conductor { get; set; }
        [Required]
        public bool Vehiculo { get; set; }
        [Required]
        public bool MuebleInmueble { get; set; }
        [Required]
        public bool Lesionado { get; set; }
        [Required]
        public bool Anulado { get; set; }

        public override string Description => Descripcion;

    }
}
