using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class RamalColorFilter : FilterPagedListFullAudited<PlaRamalColor, Int64>
    {

        public int? LineaId { get; set; }

        public override Func<PlaRamalColor, ItemDto<long>> GetItmDTO()
        {
            return e => new ItemLongDto(e.Id, e.Nombre.Trim());
        }

        public override List<Expression<Func<PlaRamalColor, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<PlaRamalColor, object>>> {
                e=> e.PlaLinea.Sucursal,
            };
        }

        public override List<Expression<Func<PlaRamalColor, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<PlaRamalColor, object>>>()
            {
               e => e.PlaLinea
            };
        }

        public override Expression<Func<PlaRamalColor, bool>> GetFilterExpression()
        {
            var exp= base.GetFilterExpression();

            if (!string.IsNullOrEmpty(this.FilterText))
            {
                exp = exp.And(e => e.Nombre.Contains(this.FilterText));
            }

            if (this.LineaId.HasValue)
            {
                exp = exp.And(e => e.LineaId ==this.LineaId);
            }

            return exp;
        }
    }
}
