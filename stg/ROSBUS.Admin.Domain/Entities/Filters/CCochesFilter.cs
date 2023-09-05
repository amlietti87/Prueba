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
    public class CCochesFilter : FilterPagedListBase<CCoches, string>
    {
        public int? UnidadNegocio { get; set; }
        
        public override Expression<Func<CCoches, bool>> GetFilterExpression()
        {
            Expression<Func<CCoches, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<CCoches, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Ficha.ToString().Trim().Contains(this.FilterText) || e.Interno.Trim().Contains(this.FilterText)
                ;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }

        public override List<Expression<Func<CCoches, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<CCoches, object>>>
            {
                e=> e.Empresa
            };
        }

        public override Func<CCoches, ItemDto<string>> GetItmDTO()
        {

            return e => new ItemDto<string>(e.Id, string.Format("{0} - {1} - {2} - {3}",e.Interno.Trim(), e.Ficha, e.Empresa?.DesEmpr.Trim(), e.Dominio));
        }
    }



}
