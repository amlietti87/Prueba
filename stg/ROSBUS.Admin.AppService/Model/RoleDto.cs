using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class RoleDto : EntityDto<int>
    {
        public string Name { get; set; }

        public Boolean IsDefault { get; set; }
        public Boolean IsStatic { get; set; }
        
        public virtual string DisplayName { get; set; }

        public override string Description => this.DisplayName;

        public bool CaducarSesionInactividad { get; set; }

    }

    public class UpdateRolPermissionsInput
    {
        public int Id { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
