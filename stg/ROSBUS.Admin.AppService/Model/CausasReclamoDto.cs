using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.ART.AppService.Model
{
    public partial class CausasReclamoDto : EntityDto<int>
    {
        [StringLength(100)]
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public override string Description => this.Descripcion;
    }
}
