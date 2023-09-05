using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class EmpresaFilter : FilterPagedListBase<Empresa, Decimal>
    {
        public Decimal? EmpresaId { get; set; }
        public bool? FecBaja { get; set; }
        public override Expression<Func<Empresa, bool>> GetFilterExpression()
        {
            Expression<Func<Empresa, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<Empresa, bool>> filterTextExp = e => true;
                filterTextExp = e => e.DesEmpr.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }
            if (!FecBaja.HasValue)
            {
                FecBaja = true;
            }
            if (FecBaja.Value)
            {

                Expression<Func<Empresa, bool>> filterTextExp = e => true;
                filterTextExp = e => e.FecBaja == null;
                Exp = Exp.And(filterTextExp);
            }

            if (EmpresaId.HasValue)
            {
                Expression<Func<Empresa, bool>> filterTextExp = e => e.Id == EmpresaId;
                Exp = Exp.Or(filterTextExp);
            }

            return Exp;
        }


    }
}
