using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class BolBanderasCartel : Entity<int>
    {
        public BolBanderasCartel()
        {
            BolBanderasCartelDetalle = new HashSet<BolBanderasCartelDetalle>();
        }

     //   public int CodBanderaCartel { get; set; }
        public int CodHfecha { get; set; }
        public int CodLinea { get; set; }
        public int? CodBanderaCartelbsas { get; set; }



        public ICollection<BolBanderasCartelDetalle> BolBanderasCartelDetalle { get; set; }
    }


    public class BolBanderasCartelDto : EntityDto<int>
    {

        public BolBanderasCartelDto()
        {
            this.BolBanderasCartelDetalle = new List<BolBanderasCartelDetalleDto>();
        }

        public int CodHfecha { get; set; }
        public int CodLinea { get; set; }
        public override string Description => "";

        public List<BolBanderasCartelDetalleDto> BolBanderasCartelDetalle { get; set; }
    }

    //public partial class BolBanderasCartelDto 
    //{
    //    public BolBanderasCartelDto()
    //    {
    //        BolBanderasCartelDetalle = new List<BolBanderasCartelDetalleDto>();
    //    }


    //    public int Id { get; set; }

    //    public int CodHfecha { get; set; }
    //    public int CodLinea { get; set; }
    //    public int? CodBanderaCartelbsas { get; set; }



    //    public List<BolBanderasCartelDetalleDto> BolBanderasCartelDetalle { get; set; }
    //}
}
