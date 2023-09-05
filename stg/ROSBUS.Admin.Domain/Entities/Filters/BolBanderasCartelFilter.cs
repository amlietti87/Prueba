using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class BolBanderasCartelFilter : FilterPagedListBase<BolBanderasCartel, int>
    {
        public int? CodHfecha { get; set; }
        public string PlanillaId { get; set; }

        public override List<Expression<Func<BolBanderasCartel, object>>> GetIncludesForPageList()
        {        
            return new List<Expression<Func<BolBanderasCartel, object>>> {
                e=> e.BolBanderasCartelDetalle,
                e=> e.BolBanderasCartelDetalle.Select(s => s.CodBanNavigation)
            };
        }

        public override List<Expression<Func<BolBanderasCartel, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<BolBanderasCartel, object>>> {
                e=> e.BolBanderasCartelDetalle                  
            };
        }

        public override Expression<Func<BolBanderasCartel, bool>> GetFilterExpression()
        {
            Expression<Func<BolBanderasCartel, bool>> Exp = base.GetFilterExpression();
            if (CodHfecha.HasValue)
            {
                Exp = Exp.And(e => e.CodHfecha == CodHfecha);
            }
            return Exp;
        }


    }
}
