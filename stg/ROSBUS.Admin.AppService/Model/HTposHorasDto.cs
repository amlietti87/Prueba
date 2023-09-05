using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HTposHorasDto :EntityDto<string>
    {
        public string DscTpoHora { get; set; }
        public int Orden { get; set; }
        public string CodTpoHorabsas { get; set; }

        public override string Description => this.DscTpoHora;
    }
}
