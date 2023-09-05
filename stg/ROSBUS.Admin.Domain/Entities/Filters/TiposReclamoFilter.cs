using ROSBUS.ART.Domain.Entities.ART;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.ART.Domain.Entities.Filters
{
    public class TiposReclamoFilter : FilterPagedListAudited<TiposReclamo, int>
    {
        public string Descripcion { get; set; }
    }
}
