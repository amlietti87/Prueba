using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysDataTypes : Entity<int>
    {
        public SysDataTypes()
        {
            SysParameters = new HashSet<SysParameters>();
        }
        public string Name { get; set; }
        public ICollection<SysParameters> SysParameters { get; set; }
    }
}
