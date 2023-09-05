using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysPermissionsUsers : Entity<long>
    {

        public long PermissionId { get; set; }
        public int UserId { get; set; }

        public SysPermissions Permission { get; set; }
        public SysUsersAd User { get; set; }
    }
}
