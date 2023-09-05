using ROSBUS.Operaciones.Domain.Entities;
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
    public class EmpleadosFilter : FilterPagedListBase<Empleados, int>
    {

        public int? UnidadNegocio { get; set; }
        public int? SucursalId { get; set; }

        public override Expression<Func<Empleados, bool>> GetFilterExpression()
        {
            Expression<Func<Empleados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                string filtertolower = this.FilterText.ToLower();
                var splitfilter = filtertolower.Split(',');

                if (splitfilter.Length == 2)
                {
                    Exp = Exp.And(e => e.Nombre.Trim().ToLower().Contains(splitfilter[1].Trim())
                    && e.Apellido.Trim().ToLower().Contains(splitfilter[0].Trim())
                    );
                }
                else
                {
                    Exp = Exp.And(e => e.Apellido.Trim().ToLower().Contains(filtertolower.Trim())
                    || e.Dni.Trim().Contains(filtertolower.Trim())
                    || e.LegajosEmpleado.Any(a => a.LegajoSap.Contains(filtertolower.Trim()))
                    );
                }


            }
            if (this.UnidadNegocio.HasValue)
            {
                Exp = Exp.And(e => e.UnidadNegocio.cod_sucursal == this.UnidadNegocio.Value);
            }

            return Exp;
        }

        public override Func<Empleados, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, string.Format("{0}, {1} - {2}", e.Apellido.Trim(), e.Nombre.Trim(), this.GetLegajosEmpleado(e)));
        }

        public string GetLegajosEmpleado(Empleados empleado)
        {
            var legajofecbaja = empleado.LegajosEmpleado.Where(f => f.FecBaja == null).Select(w => w.LegajoSap).FirstOrDefault();
            if (!String.IsNullOrEmpty(legajofecbaja))
            {
                return legajofecbaja;
            }
            else
            {

                return empleado.LegajosEmpleado.Where(f => f.FecIngreso == empleado.LegajosEmpleado.Max(u => u.FecIngreso)).Select(w => w.LegajoSap).FirstOrDefault();
            }
        }

        public override List<Expression<Func<Empleados, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<Empleados, object>>>
            {
                e=> e.LegajosEmpleado,
                e=> e.UnidadNegocio
            };
        }
    }



}
