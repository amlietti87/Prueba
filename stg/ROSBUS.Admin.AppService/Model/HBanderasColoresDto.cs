using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HBanderasColoresDto :EntityDto<int>
    {
        public override string Description => DscBanderaColor;
        public string DscBanderaColor { get; set; }
    }
}