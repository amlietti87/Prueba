using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class FactoresIntervinientesFilter : FilterPagedListFullAudited<SinFactoresIntervinientes, int>
    {
        public bool? Anulado { get; set; }
        public int? FactoresId { get; set; }
        public override Expression<Func<SinFactoresIntervinientes, bool>> GetFilterExpression()
        {
            Expression<Func<SinFactoresIntervinientes, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinFactoresIntervinientes, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinFactoresIntervinientes, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (FactoresId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.FactoresId.Value);
            }


            return Exp;
        }
        


    }
}
