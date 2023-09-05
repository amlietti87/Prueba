using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class InfResponsablesFilter : FilterPagedListBase<InfResponsables, string>
    {
        //public string CodResponsable { get; set; }

        //public string DscResponsable { get; set; }

        //public override Expression<Func<InfResponsables, bool>> GetFilterExpression()
        //{
        //    Expression<Func<InfResponsables, bool>> Exp = base.GetFilterExpression();


        //    if (!String.IsNullOrWhiteSpace(CodResponsable))
        //    {
        //        Exp = Exp.And(e => e.Id.Contains(CodResponsable));
        //    }

        //    if (!String.IsNullOrWhiteSpace(DscResponsable))
        //    {
        //        Exp = Exp.And(e => e.DscResponsable.Contains(DscResponsable));
        //    }

        //    return Exp;
        //}
    }
}
