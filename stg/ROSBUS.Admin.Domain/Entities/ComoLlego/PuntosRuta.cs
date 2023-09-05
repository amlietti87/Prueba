using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PuntosRuta
    {
        public PuntosRuta()
        {            

        }

       // [NotMapped]
        //public override int Id { get => base.Id; set => base.Id = value; }

        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public int Orden { get; set; }
        public string Data { get; set; }
        public string instructions { get; set; }


    }

    public class RutaWS9 
    {
        public List<PositionWithData> poswithdata { get; set; }
        public string Instructions { get; set; }
    }
    public class PositionWithData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Data { get; set; }

    }
}
