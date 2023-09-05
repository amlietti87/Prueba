using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HBasec
    {
        public HBasec()
        {
          
        }

        public int CodHfecha { get; set; }
        public int CodBan { get; set; }
        public int CodSec { get; set; }
        public decimal? CodGal { get; set; }
        public bool? VendeBoletos { get; set; }
        public int? CodSecbsas { get; set; }
        public int? CodRec { get; set; }
                
        


        public HBanderas CodBanNavigation { get; set; }
        public HFechasConfi CodHfechaNavigation { get; set; }
        public GpsRecorridos CodRecNavigation { get; set; }
       
    }

    public partial class HBasec
    {
        [NotMapped]
        public decimal Kmr { get; set; }
        [NotMapped]
        public decimal Km { get; set; }
        [NotMapped]
        public int? CodBanderaColor { get; set; }
        [NotMapped]
        public int? CodBanderaTup { get; set; }

        [NotMapped]
        public int? NroSecuencia { get; set; }

        [NotMapped]
        public string TextoBandera { get; set; }
        [NotMapped]
        public string Movible { get; set; }
        [NotMapped]
        public string ObsBandera { get; set; }
    }


}
