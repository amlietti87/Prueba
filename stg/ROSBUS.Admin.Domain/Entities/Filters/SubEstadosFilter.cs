using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SubEstadosFilter : FilterPagedListFullAudited<SinSubEstados, int>
    {
        public string Descripcion { get; set; }
        public bool? Anulado { get; set; }

        public int? EstadoId { get; set; }
        public int? SubEstadoId { get; set; }
        public bool? Cierre { get; set; }

        public override Expression<Func<SinSubEstados, bool>> GetFilterExpression()
        {
            Expression<Func<SinSubEstados, bool>> Exp = base.GetFilterExpression();
            if (!String.IsNullOrEmpty(this.Descripcion))
            {
                Expression<Func<SinSubEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == Descripcion;
                Exp = Exp.And(filterTextExp);
            }
            if (this.EstadoId.HasValue)
            {
                Expression<Func<SinSubEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.EstadoId == this.EstadoId;
                Exp = Exp.And(filterTextExp);
            }
            if (Anulado.HasValue)
            {
                Expression<Func<SinSubEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }
            if (Cierre.HasValue)
            {
                Expression<Func<SinSubEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Cierre == this.Cierre;
                Exp = Exp.And(filterTextExp);
            }

            if (SubEstadoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.SubEstadoId.Value);
            }



            return Exp;
        }

        public override List<Expression<Func<SinSubEstados, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinSubEstados, object>>>
            {
                e => e.Estado
            };
        }
        public override List<Expression<Func<SinSubEstados, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<SinSubEstados, object>>>
            {
                e => e.Estado
            };

        }

    }
}
