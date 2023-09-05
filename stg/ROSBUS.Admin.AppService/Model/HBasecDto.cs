using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.AppService.Model
{
    public class HBasecDto
    {
        public int CodHfecha { get; set; }
        public int CodBan { get; set; }
        public int CodSec { get; set; }
        public decimal? CodGal { get; set; }
        public bool? VendeBoletos { get; set; }
        public int? CodSecbsas { get; set; }
        public int? CodRec { get; set; }

        public string DescripcionBandera { get; set; }

        public string DescripcionSentido { get; set; }

        public string DescripcionAbreviatura { get; set; }
        public string Recorido { get; set; }


        public decimal? Kmr { get; set; }
        public decimal? Km { get; set; }
        public int? CodBanderaColor { get; set; }
        public int? CodBanderaTup { get; set; }
        public int? NroSecuencia { get; set; }


        public string TextoBandera { get; set; }
        public string Movible { get; set; }
        public string ObsBandera { get; set; }


    }


}
