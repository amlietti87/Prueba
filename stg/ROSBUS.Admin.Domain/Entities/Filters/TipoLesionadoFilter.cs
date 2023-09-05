using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class TipoLesionadoFilter : FilterPagedListFullAudited<SinTipoLesionado, int>
    {
        public string Descripcion { get; set; }

        public override Expression<Func<SinTipoLesionado, bool>> GetFilterExpression()
        {
            Expression<Func<SinTipoLesionado, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Descripcion))
            {
                Expression<Func<SinTipoLesionado, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == Descripcion;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }


        public override Func<SinTipoLesionado, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.Descripcion);
        }
    }
}
