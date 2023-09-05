using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using System.Linq;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaSentidoBanderaFilter : FilterPagedListBase<PlaSentidoBandera, int>
    {


        public int? LineaId { get; set; }

        public override Func<PlaSentidoBandera, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.Descripcion);
        }



        public override Expression<Func<PlaSentidoBandera, bool>> GetFilterExpression()
        {
            var exp= base.GetFilterExpression();

            if (this.LineaId.HasValue)
            {
                exp = e => e.HBanderas.Any(b => b.RamalColor.PlaLinea.PlaLineaLineaHoraria.Any(a => a.LineaId == this.LineaId));
            }

            return exp;
        }
    }
}
