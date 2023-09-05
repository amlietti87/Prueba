using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SysDataTypesDto : EntityDto<int>
    {
        public override string Description => Name;
        public string Name { get; set; }
    }
}
