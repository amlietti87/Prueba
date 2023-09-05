using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ProvinciasDto :EntityDto<int>
    {

        public ProvinciasDto()
        {
            //this.Localidades = new List<LocalidadesDto>();
        }
        public string DscProvincia { get; set; }
        //public int Cod_provincia { get; set; }
        public override string Description => this.DscProvincia;


        //public List<LocalidadesDto> Localidades { get; set; }
    }
}
