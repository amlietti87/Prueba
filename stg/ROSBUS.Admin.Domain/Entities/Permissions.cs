using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysPermissions : Entity<long>
    {
        public SysPermissions()
        {
            PermissionRols = new HashSet<SysPermissionsRoles>();
            PermissionsUsers = new HashSet<SysPermissionsUsers>();
            AccionesPermitidas = new HashSet<FdAccionesPermitidas>();
        }
        
        public string Area { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }


        public virtual SysAreas Areas { get; set; }

        public virtual SysPages Pages { get; set; }

        public ICollection<FdAccionesPermitidas> AccionesPermitidas { get; set; }
        public ICollection<SysPermissionsUsers> PermissionsUsers { get; set; }
       
        public virtual ICollection<SysPermissionsRoles> PermissionRols { get; set; }
    }
}
