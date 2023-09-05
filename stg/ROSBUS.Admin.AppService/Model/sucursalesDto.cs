using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class sucursalesDto :EntityDto<int>
    {
        public override string Description => this.DscSucursal;

        public string DscSucursal { get; set; }
        public string NomServidor { get; set; }
        public string EntornoActivo { get; set; }

    }
}
