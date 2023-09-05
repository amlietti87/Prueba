using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaLineaDto :EntityDto<int>
    {
        

        public override string Description => this.DesLin;

        [StringLength(100)]
        public string DesLin { get; set; }
        public int SucursalId { get; set; }
        public int PlaTipoLineaId { get; set; }

        public bool Activo { get; set; }

        public string CodRespInformes { get; set; }


        public string TipoLinea { get; set; }
    }
}
