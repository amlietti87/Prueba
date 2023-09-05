using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdAccionesPermitidasDto :EntityDto<int>
    {
        public long PermissionId { get; set; }
        public string DisplayName { get; set; }

        public string TokenPermission { get; set; }

        public override string Description => DisplayName;
    }
}
