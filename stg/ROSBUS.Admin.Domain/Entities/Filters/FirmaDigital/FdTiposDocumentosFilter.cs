using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class FdTiposDocumentosFilter : FilterPagedListAudited<FdTiposDocumentos, int>
    {
        public bool? Anulado { get; set; }
        public int? TipoDocumentoId { get; set; }
        public bool? EsPredeterminado { get; set; }
        public override Expression<Func<FdTiposDocumentos, bool>> GetFilterExpression()
        {
            Expression<Func<FdTiposDocumentos, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<FdTiposDocumentos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion.Contains(this.FilterText) ||  e.Prefijo.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<FdTiposDocumentos, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (TipoDocumentoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.TipoDocumentoId.Value);
            }

            if (EsPredeterminado.HasValue)
            {
                Exp = Exp.And(e => e.EsPredeterminado == this.EsPredeterminado);
            }


            return Exp;
        }
    }
}
