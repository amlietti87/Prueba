using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoInvolucradoFilter : FilterPagedListFullAudited<SinTipoInvolucrado, int>
    {
        public string Descripcion { get; set; }

        public override Expression<Func<SinTipoInvolucrado, bool>> GetFilterExpression()
        {
            Expression<Func<SinTipoInvolucrado, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Descripcion))
            {
                Expression<Func<SinTipoInvolucrado, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == Descripcion;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
