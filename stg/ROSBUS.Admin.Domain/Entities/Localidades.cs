using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Localidades : Entity<int>
    {

       // public int CodLocalidad { get; set; }
        public string DscLocalidad { get; set; }
        public string CodPostal { get; set; }
        public int? CodProvincia { get; set; }

         public Provincias Provincia { get; set; }

        public string GetDescription() {
            return string.Format("{0} - {1}", DscLocalidad, CodPostal);
        }

    }
}
