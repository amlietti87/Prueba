using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PracticantesFilter : FilterPagedListFullAudited<SinPracticantes, int>
    {
        public bool? Anulado { get; set; }
        public override Expression<Func<SinPracticantes, bool>> GetFilterExpression()
        {
            Expression<Func<SinPracticantes, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinPracticantes, bool>> filterTextExp = e => true;
                filterTextExp = e => e.ApellidoNombre.Contains(this.FilterText) || e.NroDoc.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinPracticantes, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }


            return Exp;
        }

        public override List<Expression<Func<SinPracticantes, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinPracticantes, object>>>
            {
                e=> e.TipoDoc,
            };
        }

        public override Func<SinPracticantes, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, string.Format("{0}", e.ApellidoNombre.Trim()));
        }
    }
}
