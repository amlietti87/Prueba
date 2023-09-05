using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysRoles : TecsoRole<SysUsersAd>
    {
        public SysRoles()
        {
            PermissionRols = new HashSet<SysPermissionsRoles>();
            UserRoles = new HashSet<SysUsersRoles>();
        }

        public ICollection<SysPermissionsRoles> PermissionRols { get; set; }
        public ICollection<SysUsersRoles> UserRoles { get; set; }

        public bool CaducarSesionInactividad { get; set; }

    }
}
