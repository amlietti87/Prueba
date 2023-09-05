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
    public class MuebleInmuebleDto :EntityDto<int>
    {
        public int TipoInmuebleId { get; set; }
        public string Lugar { get; set; }

        public int LocalidadId { get; set; }
        public String Localidad { get; set; }
        public override string Description => string.Empty;


        public ItemDto selectLocalidades { get; set; }

    }
}
