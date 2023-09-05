using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SubGalponFilter : FilterPagedListBase<SubGalpon, Decimal>
    {
        public int? CodHfecha { get; set; }


        public override Expression<Func<SubGalpon, bool>> GetFilterExpression()
        {
            var exp= base.GetFilterExpression();

            if (!String.IsNullOrWhiteSpace(this.FilterText))
            {
                exp = exp.And(e => e.DesSubg.Contains(this.FilterText));
            }


            if (this.CodHfecha.HasValue)
            {
                exp = exp.And(e => e.HHorariosConfi.Any(h => h.CodHfecha == this.CodHfecha));
            }


            return exp;
        }

        public override Func<SubGalpon, ItemDto<decimal>> GetItmDTO()
        {
            return e => new ItemDto<Decimal>(e.Id, e.DesSubg);
        }

        public override List<Expression<Func<SubGalpon, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SubGalpon, object>>>
            {
                e=> e.Configu
            };
        }
    }
}
