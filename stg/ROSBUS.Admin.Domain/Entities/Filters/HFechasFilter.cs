using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HFechasFilter : FilterPagedListBase<HFechas, int>
    {
        public int RutaID { get; set; }
        public int TipoDiaID { get; set; }
        public int? LineaId { get; set; }

        public DateTime? Fecha { get; set; }


        public override Expression<Func<HFechas, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();

            if (this.Fecha.HasValue)
            {
                var _fecha = this.Fecha.Value.Date;
                exp = exp.And(e => e.Fecha == _fecha);
            }

            if (this.LineaId.HasValue)
            {
                var _LineaId = LineaId.Value;
                exp = exp.And(e => e.Id == _LineaId);
            }


            return exp;
        }

    }
}
