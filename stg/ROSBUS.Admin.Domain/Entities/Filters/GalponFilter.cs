using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class GalponFilter : FilterPagedListFullAudited<Galpon, Decimal>
    {
        public int? SucursalId { get; set; }

        public decimal Lat { get; set; }
        public decimal Long { get; set; } 
        public string Nombre { get; set; }
 
        public override Expression<Func<Galpon, bool>> GetFilterExpression()
        {
            var exp= base.GetFilterExpression();
             
            exp = exp.And(e => e.Longitud.HasValue && e.Latitud.HasValue);

            //if (this.UnidadDeNegocioId.HasValue)
            //{
            //    exp = exp.And(e => e.UnidadDeNegocioId == this.UnidadDeNegocioId);
            //}

            return exp;
        }
    }
}
