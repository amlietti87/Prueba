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
    public class FdCertificadosFilter : FilterPagedListAudited<FdCertificados, int>
    {
        public int? UsuarioId { get; set; }
        public bool? Activo { get; set; }
        public Guid ArchivoId { get; set; }
        public string UserEmail { get; set; }

        public override Expression<Func<FdCertificados, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();
            
            if (UsuarioId.HasValue)
            {
                exp = exp.And(e => e.UsuarioId == UsuarioId);
            }

            if (Activo.HasValue)
            {
                exp = exp.And(e => e.Activo == Activo);
            }

            return exp;
            
        }

    }
}
