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
    public class GrupoLineasFilter : FilterPagedListBase<PlaGrupoLineas, int>
    {

  
        public int? SucursalId { get; set; }
         
        public ItemLongDto Linea  { get; set; }

        public override Expression<Func<PlaGrupoLineas, bool>> GetFilterExpression()
        {

            Expression<Func<PlaGrupoLineas, bool>> baseFE = base.GetFilterExpression();
            
            if (this.SucursalId.HasValue)
            {
                baseFE = baseFE.And(e => e.SucursalId == SucursalId.Value);
            }

            if (this.Linea != null && this.Linea.Id > 0)
            {
                baseFE = baseFE.And(e => e.Linea.Any(a => a.Id == this.Linea.Id));
            }

            return baseFE;
        }

        public override List<Expression<Func<PlaGrupoLineas, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<PlaGrupoLineas, object>>> {
                e=> e.Sucursal,
                e=> e.Linea
            };
        }
    }
    
}
