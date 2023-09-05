using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using System.Linq;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class FdEstadosFilter : FilterPagedListBase<FdEstados, int>
    {
        public bool? MostrarBDEmpleado { get; set; }

        public override Expression<Func<FdEstados, bool>> GetFilterExpression()
        {
            Expression<Func<FdEstados, bool>> Exp = base.GetFilterExpression();

            if (MostrarBDEmpleado.HasValue && MostrarBDEmpleado == true)
            {
                Exp = Exp.And(e => e.FdAccionesEstadoActual.Any(c => c.MostrarBdempleado == true));
                Exp = Exp.Or(e => e.FdAccionesEstadoNuevo.Any(c => c.MostrarBdempleado == true)); 

            }


            return Exp;
        }
    }
}
