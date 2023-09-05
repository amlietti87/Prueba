using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Provincias : Entity<int>
    {

        public Provincias()
        {
            Localidades = new HashSet<Localidades>();
        }
        //public int CodProvincia { get; set; }
        public string DscProvincia { get; set; }
        public ICollection<Localidades> Localidades { get; set; }
    }
}
