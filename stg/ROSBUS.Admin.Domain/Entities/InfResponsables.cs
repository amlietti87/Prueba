using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class InfResponsables : Entity<string>
    {
        public InfResponsables()
        {
            Lineas = new HashSet<Linea>();
        }

        public override string Id { get; set; }
        public string DscResponsable { get; set; }
        public string CodResponsableBa { get; set; }

        public ICollection<Linea> Lineas { get; set; }
    }
}
