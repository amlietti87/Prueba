using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysUsersRoles : CreationAuditedEntity<long>
    {
       
        public SysUsersRoles()
        {

        }

        public int RoleId { get; set; }
        public int UserId { get; set; }

        public SysRoles Role { get; set; }
        public SysUsersAd User { get; set; }
    }

    public partial class SysUsersRoles
    {
        public const int admin = 1;      
        public const int Inspector = 7;
    }
}
