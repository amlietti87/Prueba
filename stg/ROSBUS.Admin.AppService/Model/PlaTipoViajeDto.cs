using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaTipoViajeDto : EntityDto<int>
    {
        public string TravelMode { get; set; }
        public string Descripcion { get; set; }
        public override string Description => Descripcion;
    }
}
