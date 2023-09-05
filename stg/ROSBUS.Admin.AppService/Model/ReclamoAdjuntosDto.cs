using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ReclamoAdjuntosDto :EntityDto<long>
    {
        public int ReclamoId { get; set; }

        public Guid AdjuntoId { get; set; }
        public override string Description => string.Empty;

    }
}
