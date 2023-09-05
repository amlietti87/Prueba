using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysPermissionsRoles : Entity<long>
    {

        public long PermissionId { get; set; }
        public int RolId { get; set; }

      

        public SysPermissions Permission { get; set; }
        public SysRoles Rol { get; set; }

    }
}
