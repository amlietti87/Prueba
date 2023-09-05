using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysPages
    {
        public SysPages()
        {
            Permissions = new HashSet<SysPermissions>();
        }

        public string Page { get; set; }
        public string DisplayName { get; set; }

        public ICollection<SysPermissions> Permissions { get; set; }
    }
}
