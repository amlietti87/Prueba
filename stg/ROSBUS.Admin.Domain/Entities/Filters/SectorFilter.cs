using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SectorFilter : FilterPagedListBase<PlaSector, Int64>
    {


        public int? RutaId { get; set; }

     
        public override Expression<Func<PlaSector, bool>> GetFilterExpression()
        {
            Expression<Func<PlaSector, bool>> Exp = base.GetFilterExpression();

            if (RutaId.HasValue)
            {
                var rutaId = RutaId.Value;
                Exp = Exp.And(e => e.CodRec == rutaId);
            }

            return Exp;
        }

        public override Func<PlaSector, ItemDto<long>> GetItmDTO()
        {
            return e => new ItemLongDto(e.Id, e.Descripcion);
        }

    }


    public class SectorConPuntosFilter : SectorFilter
    {

        public override List<Expression<Func<PlaSector, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<PlaSector, object>>> {
                e=> e.PuntoInicio,
                e=> e.PuntoFin
            };


        }




    }
}
