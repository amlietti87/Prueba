using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HBanderas : FullAuditedEntity<int>
    {
        public HBanderas()
        {
            Rutas = new HashSet<GpsRecorridos>();
            PlaCodigoSubeBandera = new HashSet<PlaCodigoSubeBandera>();
            HBasec = new HashSet<HBasec>();
            HMinxtipo = new HashSet<HMinxtipo>();
            HMediasVueltas = new HashSet<HMediasVueltas>();
            BolBanderasCartelDetalle = new HashSet<BolBanderasCartelDetalle>();

        }

        //public int CodBan { get; set; }
        public string DesBan { get; set; }
        public string AbrBan { get; set; }
        public string SenBan { get; set; }
        public string ClaveWay { get; set; }
        public decimal? Velocidad { get; set; }
        public decimal? Kmr { get; set; }
        public decimal? Km { get; set; }
        public long? RamalColorId { get; set; }
        public string CodigoVarianteLinea { get; set; }
        public string Ramalero { get; set; }
        public bool? Activo { get; set; }
        
        public int TipoBanderaId { get; set; }        
        public string PorDonde { get; set; }
        public HBanderasEspeciales BanderasEspeciales { get; set; }

        public PlaRamalColor RamalColor { get; set; }

        public PlaSentidoBandera SentidoBandera { get; set; }
        

        public ICollection<GpsRecorridos> Rutas { get; set; }

        public ICollection<PlaCodigoSubeBandera> PlaCodigoSubeBandera { get; set; }

        public ICollection<HBasec> HBasec { get; set; }
        public ICollection<HMinxtipo> HMinxtipo { get; set; }

        public ICollection<BolBanderasCartelDetalle> BolBanderasCartelDetalle { get; set; }
        public ICollection<HMediasVueltas> HMediasVueltas { get; set; }

        public int? SucursalId { get; set; }

        public int? SentidoBanderaId { get; set; }

        public string Origen { get; set; }

        public string Destino { get; set; }

        public string DescripcionPasajeros { get; set; }



    }

    public partial class HBanderasEspeciales
    {
        public int CodBan { get; set; }
        public bool? cortado { get; set; }

        public HBanderas HBanderas { get; set; }
    }




    public partial class HBanderas {

        [NotMapped]
        public Boolean Cortado { get; set; }
    }
}
