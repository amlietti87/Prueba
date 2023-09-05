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
    public class HFechasConfiFilter : FilterPagedListBase<HFechasConfi, int>
    {

        public int? cod_hfecha { get; set; }

        public int? cod_ban{ get; set; }


        public DateTime? fec_desde { get; set; }


        public decimal? LineaId { get; set; }

        public bool? CopyConductores { get; set; }

        public override Expression<Func<HFechasConfi, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();
            
            if (this.LineaId.HasValue)
            {
                var _LineaId = LineaId.Value;
                exp = exp.And(e => e.CodLin == _LineaId);
            }
            return exp;
        }


        public override List<Expression<Func<HFechasConfi, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<HFechasConfi, object>>>() {
                e=> e.PlaDistribucionDeCochesPorTipoDeDia
            };
        }

        //public override List<Expression<Func<HFechasConfi, object>>> GetIncludesForPageList()
        //{
        //    return new List<Expression<Func<HFechasConfi, object>>>() {
        //        e=> e.HHorariosConfi,
        //        e=> e.PlaEstadoHorarioFecha
        //    };
        //}

        public override Func<HFechasConfi, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.FecDesde.ToString("dd/MM/yyyy"));
            
        }
    }

    public class ExportarExcelFilter
    {
        public int? CodTdia { get; set; }

        public Decimal? CodSubg { get; set; }

        public int CodHfecha { get; set; }
    }
}
