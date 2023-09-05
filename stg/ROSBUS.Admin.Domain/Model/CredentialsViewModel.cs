using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ROSBUS.Admin.Domain.Model
{
    public class CredentialsViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string CaptchaValue { get; set; }
    }

    public class CredentialsIntrabusModel 
    {
        [Required]
        public string AccesToken { get; set; }
    }


    
}
