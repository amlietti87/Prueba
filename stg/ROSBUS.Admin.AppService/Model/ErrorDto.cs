using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ErrorDto : EntityDto<long>
    {
        public override string Description => "";

        public DateTime ErrorDate { get; set; }

        public String UserName { get; set; }

        public String SessionId { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }


    }
}
