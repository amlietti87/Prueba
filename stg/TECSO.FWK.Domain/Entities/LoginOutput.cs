using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    public class LoginOutput
    {

        public string username { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string token { get; set; }
        public DateTime expires { get; set; }
        public int? sucursalId { get; set; }
        public int? empleadoId { get; set; }
        public int sessionTimeout { get; set; }

    }
}
