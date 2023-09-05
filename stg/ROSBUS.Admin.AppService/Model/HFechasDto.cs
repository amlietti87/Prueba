using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HFechasDto :EntityDto<int>
    {      
        public override string Description => this.Fecha.ToString();
        public DateTime Fecha { get; set; }
        public int CodTdia { get; set; }
        public string Feriado { get; set; }
        public string CompensatorioPago { get; set; }


    }
}
