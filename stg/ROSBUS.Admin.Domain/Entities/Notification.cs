using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Notification : Entity<int>
    {
        public string Descripcion { get; set; }
        public string Token { get; set; }
        public string Permiso { get; set; }

        public ICollection<InspGruposInspectores> InspGruposInspectores { get; set; }
    }
}
