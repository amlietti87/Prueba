using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HServiciosDto :EntityDto<int>
    {
        public HServiciosDto()
        {
            this.HMediasVueltas = new List<HMediasVueltasDto>();
        }

        public int CodHconfi { get; set; }
        public string NumSer { get; set; } 
        public string NroInterno { get; set; }
        public int Duracion { get; set; }

        public override string Description => this.NumSer;
        
        public List<HMediasVueltasDto> HMediasVueltas { get; set; }
    }
}
