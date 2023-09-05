using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class LocalidadesDto :EntityDto<int>
    {
        public string DscLocalidad { get; set; }
        public string CodPostal { get; set; }
        public int? CodProvincia { get; set; }

        public string ProvinciaName { get; set; }
        public override string Description => string.Empty;

    }
}
