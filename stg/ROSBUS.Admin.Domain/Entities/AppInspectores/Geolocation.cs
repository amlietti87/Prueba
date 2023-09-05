using System;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class InspGeo : Entity<long>
    {

        public string UserName { get; set; }
        public DateTime CurrentTime { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Accion { get; set; }

    }


    public partial class InspGeo_Hist : Entity<long>
    
    {
        public string UserName { get; set; }
        public DateTime CurrentTime { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Accion { get; set; }
    }
}
