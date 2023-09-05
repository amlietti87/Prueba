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
    public class LineaFilter : FilterPagedListFullAudited<Linea, decimal>
    {
        public int? SucursalId { get; set; }

        public bool? Activo { get; set; }
        public int? EmpresaId { get; set; }
        public int? TipoLineaId { get; set; }
        public long? GrupoLineaId { get; set; }

        public int? UserId { get; set; }
        public bool SinFechaBaja { get; set; }
        public bool IncludePlaLineaHoraria { get; set; }

        public override Func<Linea, ItemDto<decimal>> GetItmDTO()
        {
            return e => new ItemDto<decimal>(e.Id, e.DesLin.Trim());
        }

        public override Expression<Func<Linea, bool>> GetFilterExpression()
        {


            Expression<Func<Linea, bool>> baseExp = base.GetFilterExpression();

            if (SinFechaBaja)
            {
                baseExp = baseExp.And(e => e.FecBaja == null);
            }


            if (Activo.HasValue)
            {
                baseExp = baseExp.And(e => e.Activo == Activo.Value);
            }

            if (SucursalId.HasValue)
            {
                var sucursalId = SucursalId.Value;
                baseExp = baseExp.And(e => e.SucursalesxLineas.Any(a => a.Id == sucursalId));
            }

           
            if (GrupoLineaId.HasValue)
            {
                var grupolineaId = GrupoLineaId.Value;
                baseExp = baseExp.And(e => e.PlaGrupoLineaId == grupolineaId);
            }


            if (TipoLineaId.HasValue)
            {
                var urn_int = TipoLineaId.Value.ToString();
                baseExp = baseExp.And(e => e.UrbInter == urn_int);
            }


            if (this.UserId.HasValue)
            {
                var userId = this.UserId.Value;
                baseExp = baseExp.And(e => e.PlaLineasUsuarios.Any(a => a.UserId == userId));
            }

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<Linea, bool>> filterTextExp = e => true;
                filterTextExp = e => e.DesLin.Contains(this.FilterText);
                baseExp = baseExp.And(filterTextExp);
            }

            return baseExp;
        }


        public override List<Expression<Func<Linea, object>>> GetIncludesForGetById()
        {
            var lst= new List<Expression<Func<Linea, object>>>
            {
                e=> e.SucursalesxLineas,
            };

            if (this.IncludePlaLineaHoraria)
            {
                lst.Add(e => e.PlaLineaLineaHoraria.Select(j=> j.PlaLinea));
            }
                

            return lst;
        }

        public override List<Expression<Func<Linea, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<Linea, object>>>
            {                
                e=> e.PlaGrupoLinea,
                e=> e.SucursalesxLineas,
               // e=> e.Grupolinea
            };
        }
    }
}
