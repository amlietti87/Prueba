using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysParameters : Entity<long>
    {

        

        public string Token { get; set; }
        public string Value { get; set; }
        public string  Description { get; set; }
        public int? DataTypeId { get; set; }
        public SysDataTypes SysDataType { get; set; }
    }




}
