using System;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class Error: Entity<Int64>
    {
        public DateTime ErrorDate { get; set; }

        public String UserName { get; set; }

        public String ErrorMessage { get; set; }

        public String SessionId { get; set; }

        public string StackTrace { get; set; }

    }
}
