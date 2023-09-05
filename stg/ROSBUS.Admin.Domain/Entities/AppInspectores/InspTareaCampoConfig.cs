using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class InspTareaCampoConfig : Entity<int>
    {
        public InspTareaCampoConfig()
        {
            TareasCampos = new HashSet<InspTareaCampo>();
        }
        public string Campo { get; set; }
        public string Descripcion { get; set; }

        public ICollection<InspTareaCampo> TareasCampos { get; set; }
    }
}
