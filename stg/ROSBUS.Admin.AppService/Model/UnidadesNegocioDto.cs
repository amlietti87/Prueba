using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class UnidadesNegocioDto : EntityDto<string>
    {
        public string UN { get; set; }
        public string descripcion { get; set; }
        public int? cod_sucursal { get; set; }

        public override string Description => string.Empty;
    }
}
