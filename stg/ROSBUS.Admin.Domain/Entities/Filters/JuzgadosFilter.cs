using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class JuzgadosFilter : FilterPagedListFullAudited<SinJuzgados, int>
    {
        public string Descripcion { get; set; }

        public override Expression<Func<SinJuzgados, bool>> GetFilterExpression()
        {
            Expression<Func<SinJuzgados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinJuzgados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
