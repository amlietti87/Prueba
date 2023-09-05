using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{


    public class FdDocumentosErrorFilter : FilterPagedListBase<FdDocumentosError, long>
    {

        public int? Revisado { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public override Expression<Func<FdDocumentosError, bool>> GetFilterExpression()
        {
            Expression<Func<FdDocumentosError, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<FdDocumentosError, bool>> filterTextExp = e => true;
                filterTextExp = e => e.TipoDocumento.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }



            if (FechaDesde.HasValue)
            {
                Expression<Func<FdDocumentosError, bool>> filterTextExp = e => e.FechaProcesado.HasValue && e.FechaProcesado.Value.Date >= FechaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaHasta.HasValue)
            {
                Expression<Func<FdDocumentosError, bool>> filterTextExp = e => e.FechaProcesado.HasValue && e.FechaProcesado.Value.Date <= FechaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }




            if (Revisado.HasValue)
            {
                if (Revisado.Value == 1)
                {
                    Expression<Func<FdDocumentosError, bool>> filterTextExp = e => e.Revisado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (Revisado.Value == 2)
                {
                    Expression<Func<FdDocumentosError, bool>> filterTextExp = e => e.Revisado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            return Exp;
        }

        public override List<Expression<Func<FdDocumentosError, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<FdDocumentosError, object>>>() {
                e => e.TipoDocumento,
                e => e.Sucursal,
                e => e.Empresa
            };
        }
    }
}
