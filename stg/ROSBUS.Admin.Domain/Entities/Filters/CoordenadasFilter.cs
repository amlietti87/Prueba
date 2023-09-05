using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class CoordenadasFilter : FilterPagedListFullAudited<PlaCoordenadas, int>
    {
        public string Abreviacion { get; set; }
        public string CodigoNombre { get; set; }


        public string DescripcionCalle1 { get; set; }
        public string DescripcionCalle2 { get; set; }
        public string Calle1 { get; set; }
        public string Calle2 { get; set; }

        public int? AnuladoCombo { get; set; }
        public DateTime? Fecha { get; set; }

        public override Func<PlaCoordenadas, ItemDto<int>> GetItmDTO()
        {
            return GetItmDTOFunc();
        }

        public static Func<PlaCoordenadas, ItemDto<int>> GetItmDTOFunc()
        {
            return e => new ItemDto<int>(e.Id, e.GetDescription());
        }


        public override Expression<Func<PlaCoordenadas, bool>> GetFilterExpression()
        {
            Expression<Func<PlaCoordenadas, bool>> Exp = base.GetFilterExpression();
            Exp = Exp.And(e => e.BeforeMigration == false);
            
            if (!String.IsNullOrEmpty(this.CodigoNombre))
            {

                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.CodigoNombre == CodigoNombre;
                Exp = Exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.Abreviacion))
            {

                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.Abreviacion == Abreviacion;
                Exp = Exp.And(filterTextExp);
            }


            if (!String.IsNullOrEmpty(this.FilterText))
            {
                this.FilterText = this.FilterText.ToLower();
                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => (e.Calle1.ToLower().Trim() + "-" + e.Calle2.ToLower().Trim()).StartsWith(FilterText);

                var split = this.FilterText.Split("-");
                if (split.Length == 1 || (split.Length == 2 && String.IsNullOrWhiteSpace(split[1])))
                {
                    Expression<Func<PlaCoordenadas, bool>> filterTextExp1 = e => e.DescripcionCalle1.RemoveDiacritics().ToLower().Trim().StartsWith(split[0]);
                    filterTextExp = filterTextExp.Or(filterTextExp1);
                }
                else if (split.Length == 2 && !String.IsNullOrWhiteSpace(split[1]))
                {
                    Expression<Func<PlaCoordenadas, bool>> filterTextExp2 = e => e.DescripcionCalle1.RemoveDiacritics().ToLower().Trim().StartsWith(split[0]) && e.DescripcionCalle2.RemoveDiacritics().ToLower().Trim().StartsWith(split[1]);
                    filterTextExp = filterTextExp.Or(filterTextExp2);
                }

                Exp = Exp.And(filterTextExp);

            }


            if (!String.IsNullOrEmpty(this.Calle1))
            {
                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.Calle1.ToLower().StartsWith(Calle1.ToLower());
                Exp = Exp.And(filterTextExp);
            }


            if (!String.IsNullOrEmpty(this.Calle2))
            {
                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.Calle2.ToLower().StartsWith(Calle2.ToLower());
                Exp = Exp.And(filterTextExp);
            }



            if (!String.IsNullOrEmpty(this.DescripcionCalle1))
            {
                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.DescripcionCalle1.RemoveDiacritics().ToLower().Trim().Contains(DescripcionCalle1.ToLower());
                Exp = Exp.And(filterTextExp);
            }


            if (!String.IsNullOrEmpty(this.DescripcionCalle2))
            {
                Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.DescripcionCalle2.RemoveDiacritics().ToLower().Trim().Contains(DescripcionCalle2.ToLower());
                Exp = Exp.And(filterTextExp);
            }

            if (AnuladoCombo.HasValue)
            {
                if (AnuladoCombo.Value == 1)
                {
                    Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.Anulado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (AnuladoCombo.Value == 2)
                {
                    Expression<Func<PlaCoordenadas, bool>> filterTextExp = e => e.Anulado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            return Exp;
        }



    }
}
