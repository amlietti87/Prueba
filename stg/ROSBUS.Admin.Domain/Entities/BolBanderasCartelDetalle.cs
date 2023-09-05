using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class BolBanderasCartelDetalle
    {
        public int CodBanderaCartel { get; set; }
        public int CodBan { get; set; }
        public int NroSecuencia { get; set; }
        public string TextoBandera { get; set; }
        public string Movible { get; set; }
        public string ObsBandera { get; set; }

        public HBanderas CodBanNavigation { get; set; }

        public BolBanderasCartel CodBanderaCartelNavigation { get; set; }
    }

    public class BolBanderasCartelDetalleDto
    {
        public int CodBanderaCartel { get; set; }
        public int CodBan { get; set; }
        public int NroSecuencia { get; set; }
        public string TextoBandera { get; set; }
        public string Movible { get; set; }
        public string ObsBandera { get; set; }
        public string AbrBan { get; set; }
        public Boolean EsPosicionamiento { get; set; }

    }
}
