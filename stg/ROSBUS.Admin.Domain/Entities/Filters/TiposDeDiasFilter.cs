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
    public class TiposDeDiasFilter : FilterPagedListFullAudited<HTipodia, int>
    {

        public int? CodHfecha { get; set; }

        public int? TipoDiaId { get; set; }

        public override Func<HTipodia, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.DesTdia);
        }



        public override Expression<Func<HTipodia, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();

            if (CodHfecha.HasValue)
            {
                basequery = basequery.And(e => e.PlaDistribucionDeCochesPorTipoDeDia.Any(a => a.CodHfecha == CodHfecha));
            }

            return basequery;
        }

    }
}
