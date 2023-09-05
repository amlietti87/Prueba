using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HServiciosFilter : FilterPagedListBase<HServicios, int>
    {
        public DateTime? Fecha { get; set; }
        public decimal? LineaId { get; set; }
        public int? ServicioId { get; set; }
        public string Nombre { get; set; }

        public string ConductorId { get; set; }
        public int? UserIdInspector { get; set; }


        public int? CodHfecha { get; set; }
        public int? CodTdia { get; set; }
        public decimal? CodSubg { get; set; }

        public override List<Expression<Func<HServicios, object>>> GetIncludesForPageList()
        {
            return base.GetIncludesForPageList();
        }


        public override Expression<Func<HServicios, bool>> GetFilterExpression()
        {
            var exp= base.GetFilterExpression();


            if (CodHfecha.HasValue)
            {
                exp = exp.And(e => e.CodHconfiNavigation.CodHfecha == this.CodHfecha);
            }

            if (CodTdia.HasValue)
            {
                exp = exp.And(e => e.CodHconfiNavigation.CodTdia == this.CodTdia);
            }

            if (CodSubg.HasValue)
            {
                exp = exp.And(e => e.CodHconfiNavigation.CodSubg == this.CodSubg);
            }

            return exp;
        }


        public override Func<HServicios, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.NumSer);
        }
    }
}
