using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HDesignarFilter : FilterPagedListBase<HDesignar, int>
    {
        public DateTime Fecha { get; set; }

        public int Sector { get; set; }

        public int Sentido { get; set; }

        public int TipoLinea { get; set; }

    }
}
