using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PlaParadasFilter : FilterPagedListBase<PlaParadas, int>
    {
        public bool? Anulada { get; set; }
        public string Codigo { get; set; }
        public string Calle { get; set; }
        public string Cruce { get; set; }
        public int? LocalidadId { get; set; }
        public string Sentido { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }
        public int? LocationType { get; set; }
        public int? AnuladoCombo { get; set; }
        public int? ParentStationId { get; set; }





        public Boolean? SoloParadasAsociadasALineas { get; set; }
        public DateTime? Fecha { get; set; }
        public int? LineaId { get; set; }
        public List<int> Rutas { get; set; }




        public override Func<PlaParadas, ItemDto<int>> GetItmDTO()
        {
            return e => new ItemDto<int>(e.Id, e.GetDescription());
        } 
        public override Expression<Func<PlaParadas, bool>> GetFilterExpression()
        {
            var exp = base.GetFilterExpression();
            if (this.Anulada.HasValue)
            {
                exp = exp.And(e => e.Anulada == this.Anulada.Value);
            }

            if (!this.FilterText.IsNullOrEmpty())
            {
                exp = exp.And(e => e.Codigo.Contains(this.FilterText) ||
                e.Cruce.Contains(this.FilterText) ||
                e.Calle.Contains(this.FilterText) ||
                e.Sentido.Contains(this.FilterText)
                );
            }


            if (!String.IsNullOrEmpty(this.Calle))
            {
                
                Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Calle.RemoveDiacritics().ToLower().Contains(Calle.RemoveDiacritics().ToLower());
                exp = exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.Sentido))
            {
                Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Sentido.RemoveDiacritics().ToLower().Contains(Sentido.RemoveDiacritics().ToLower());
                exp = exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.Cruce))
            {

                Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Cruce.RemoveDiacritics().ToLower().Contains(Cruce.RemoveDiacritics().ToLower());
                exp = exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.Codigo))
            {
                Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Codigo == this.Codigo;
                exp = exp.And(filterTextExp);
            }

            if (this.LocalidadId.HasValue)
            {
                Expression<Func<PlaParadas, bool>> filterTextExp = e => e.LocalidadId == this.LocalidadId;
                exp = exp.And(filterTextExp);
            }

            if (AnuladoCombo.HasValue)
            {
                if (AnuladoCombo.Value == 1)
                {
                    Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Anulada == true;
                    exp = exp.And(filterTextExp);
                }
                else if (AnuladoCombo.Value == 2)
                {
                    Expression<Func<PlaParadas, bool>> filterTextExp = e => e.Anulada == false;
                    exp = exp.And(filterTextExp);
                }
            }
            if (this.LocationType.HasValue)
            {
                exp = exp.And(e => e.LocationType == this.LocationType.Value);
            }
            if (this.ParentStationId.HasValue)
            {
                exp = exp.And(e => e.ParentStationId == this.ParentStationId.Value);
            }

            if (this.SoloParadasAsociadasALineas.GetValueOrDefault())
            {
                exp = exp.And(e => e.PlaPuntos.Any(p => p.EsParada && this.Rutas.Contains(p.CodRec)));
            }

            return exp;
        }


    }
}
