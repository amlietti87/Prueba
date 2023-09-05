using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoMuebleFilter : FilterPagedListFullAudited<SinTipoMueble, int>
    {
        public string Descripcion { get; set; }

        public override Expression<Func<SinTipoMueble, bool>> GetFilterExpression()
        {
            Expression<Func<SinTipoMueble, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Descripcion))
            {
                Expression<Func<SinTipoMueble, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == Descripcion;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }



    }
}
