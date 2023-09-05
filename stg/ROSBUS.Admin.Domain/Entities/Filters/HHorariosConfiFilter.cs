using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HHorariosConfiFilter : FilterPagedListBase<HHorariosConfi, int>
    {

        public int? CodHfecha { get; set; }
        public int? CodTdia { get; set; }
        public decimal? CodSubg { get; set; }

        public int? ServicioId { get; set; }


        public override Expression<Func<HHorariosConfi, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();


            if (CodHfecha.HasValue)
            {
                exp = exp.And(e => e.CodHfecha == this.CodHfecha);
            }

            if (CodTdia.HasValue)
            {
                exp = exp.And(e => e.CodTdia == this.CodTdia);
            }

            if (CodSubg.HasValue)
            {
                exp = exp.And(e => e.CodSubg == this.CodSubg);
            }



            return exp;
        }

        public override Func<HHorariosConfi, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, string.Empty);
        }

    }
}
