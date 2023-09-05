using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public partial class InspGeoDto : EntityDto<long>
    {

        public string UserName { get; set; }
        public DateTime CurrentTime { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Accion { get; set; }

        public override string Description => string.Empty;
    }


    public class GeolocalizationResponse
    {

        public Boolean Status { get; set; }

        public String Messages { get; set; }
    }
}
