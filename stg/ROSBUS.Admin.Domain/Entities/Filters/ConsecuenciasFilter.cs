using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class ConsecuenciasFilter : FilterPagedListFullAudited<SinConsecuencias, int>
    {
        public string Descripcion { get; set; }

        public Boolean? FiltrarAnulado { get; set; }

        public override Expression<Func<SinConsecuencias, bool>> GetFilterExpression()
        {
            Expression<Func<SinConsecuencias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinConsecuencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }
            if (FiltrarAnulado.HasValue && FiltrarAnulado.Value == true)
            {
                Expression<Func<SinConsecuencias, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == false;
                Exp = Exp.And(filterTextExp);
            }

                return Exp;
        }

        public override List<Expression<Func<SinConsecuencias, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinConsecuencias, object>>>
            {
                e=> e.SinCategorias
            };
        }


    }
}
