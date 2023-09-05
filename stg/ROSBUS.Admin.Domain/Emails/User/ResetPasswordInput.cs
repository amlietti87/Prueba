using ROSBUS.Admin.Domain.Emailing;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.bus;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Mail;

namespace ROSBUS.Admin.Domain
{
    public class ResetPasswordInput
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public string ResetCode { get; set; }

        [Required]        
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }
    }
    public class ResetPasswordOutput
    {
        public bool CanLogin { get; set; }

        public string UserName { get; set; }
    }
}
