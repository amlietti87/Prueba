using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaLineaFilter : FilterPagedListFullAudited<PlaLinea, int>
    {
        public int? SucursalId { get; set; }

        public bool? Activo { get; set; }
        public int? EmpresaId { get; set; }
        public int? TipoLineaId { get; set; }





        public override Func<PlaLinea, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.DesLin.Trim());
        }



        public override Expression<Func<PlaLinea, bool>> GetFilterExpression()
        {


            Expression<Func<PlaLinea, bool>> baseExp = base.GetFilterExpression();

            if (Activo.HasValue)
            {
                baseExp = baseExp.And(e => e.Activo == Activo.Value);
            }

            if (SucursalId.HasValue)
            {
                var sucursalId = SucursalId.Value;
                baseExp = baseExp.And(e => e.SucursalId == sucursalId);
            }


            if (TipoLineaId.HasValue)
            {
                baseExp = baseExp.And(e => e.PlaTipoLineaId == TipoLineaId);
            }


            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<PlaLinea, bool>> filterTextExp = e => true;
                filterTextExp = e => e.DesLin.Contains(this.FilterText);
                baseExp = baseExp.And(filterTextExp);
            }

            return baseExp;
        }


        public override List<Expression<Func<PlaLinea, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<PlaLinea, object>>>
            {
                e=> e.Sucursal,
            };
        }

        public override List<Expression<Func<PlaLinea, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<PlaLinea, object>>>
            {
                e=> e.PlaTipoLinea,
                e=> e.Sucursal,
               // e=> e.Grupolinea
            };
        }
    }
}
