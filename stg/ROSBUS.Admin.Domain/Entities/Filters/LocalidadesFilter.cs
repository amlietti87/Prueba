using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class LocalidadesFilter : FilterPagedListBase<Localidades, int>
    {
        public string DscLocalidad { get; set; }

        public int? LocalidadId { get; set; }

        public int? ProvinciaId { get; set; }

        public int? CodProvincia { get; set; }


        public override Expression<Func<Localidades, bool>> GetFilterExpression()
        {
            Expression<Func<Localidades, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            { 
                Exp = Exp.And(e => e.DscLocalidad.ToLower().StartsWith(FilterText.ToLower()) ||
                e.CodPostal == FilterText);
            }
            return Exp;
        }

        public override Func<Localidades, ItemDto<int>> GetItmDTO()
        {
            
            return e => new ItemDto<int>(e.Id, string.Format("{0} - {1}", e.DscLocalidad , e.CodPostal));
        }

        public override List<Expression<Func<Localidades, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<Localidades, object>>>
            {
                e=> e.Provincia,
            };
        }

    }
}
