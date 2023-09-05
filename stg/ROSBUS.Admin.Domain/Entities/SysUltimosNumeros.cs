using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysUltimosNumeros : Entity<string>
    {
        public string Tabla { get; set; }
        public string Tipo1 { get; set; }
        public string Tipo2 { get; set; }
        public string Tipo3 { get; set; }
        public string Tipo4 { get; set; }
        public int UltNumero { get; set; }
        public Guid Rowguid { get; set; }

        

    }
}
