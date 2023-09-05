using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class InfResponsablesDto : EntityDto<string>
    {
        [StringLength(50)]
        public string DscResponsable { get; set; }

        public override string Description => this.DscResponsable;
    }
}
