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
    public class InspTareaFilter : FilterPagedListBase<InspTarea, int>
    {
        public string Descripcion { get; set; }
        public bool? Anulado { get; set; }

        public override Expression<Func<InspTarea, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                basequery = basequery.And(e => e.Descripcion.Contains(this.FilterText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(Descripcion))
            {
                basequery = basequery.And(e=> e.Descripcion.Contains(Descripcion, StringComparison.OrdinalIgnoreCase));
            }

            if (Anulado.HasValue)
            {
                Expression<Func<InspTarea, bool>> filterTextExp = e => e.Anulado == this.Anulado;
                basequery = basequery.And(filterTextExp);               
            }


            return basequery;
        }

    }
}
