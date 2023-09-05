using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class GpsDetaReco
    {
        public int CodRec { get; set; }
        public int Cuenta { get; set; }
        public string Sent1 { get; set; }
        public string Sent2 { get; set; }
        public decimal Metro { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public string Sector { get; set; }
        public string DscSector { get; set; }
    }
}
