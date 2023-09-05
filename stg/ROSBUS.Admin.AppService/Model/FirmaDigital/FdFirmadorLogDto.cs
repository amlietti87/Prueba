using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdFirmadorLogDto :EntityDto<long>
    {
        public long FirmadorId { get; set; }
        public string DetalleLog { get; set; }
        public DateTime FechaHora { get; set; }

        public override string Description => "";
    }
}
