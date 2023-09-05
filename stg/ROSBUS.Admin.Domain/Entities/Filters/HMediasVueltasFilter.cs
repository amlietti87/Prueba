using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HMediasVueltasFilter : FilterPagedListBase<HMediasVueltas, int>
    {
        public int? CodServicio { get; set; }
        public int? CodHfecha { get; set; }
        public int? CodTdia { get; set; }

        public decimal? CodSubg { get; set; }
        public decimal? CodLinea { get; set; }
        public List<HServicios> Servicios { get; set; }


        public override List<Expression<Func<HMediasVueltas, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<HMediasVueltas, object>>>
            {
                e=> e.CodBanNavigation,
                e=> e.CodTpoHoraNavigation
            };
        }


        public override Expression<Func<HMediasVueltas, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();

            if (this.CodServicio.HasValue)
            {
                exp = exp.And(e => e.CodServicio == this.CodServicio);
            }

            return exp;
        }
    }
}
