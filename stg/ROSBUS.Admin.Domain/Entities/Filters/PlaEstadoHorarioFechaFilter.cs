using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaEstadoHorarioFechaFilter : FilterPagedListBase<PlaEstadoHorarioFecha, int>
    {


        

        public override Func<PlaEstadoHorarioFecha, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.Descripcion);
        }
    }
}
