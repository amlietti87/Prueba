using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class HMinxtipoFilter : FilterPagedListBase<HMinxtipo, int>
    {
        //cod_hfecha =7040 and cod_tdia = 5 and cod_ban = 546 and tipo_hora = 'E'

        public int? CodHfecha { get; set; }
        public int? CodTdia { get; set; }
        public int? CodBan { get; set; }
        public string TipoHora { get; set; }

        public string PlanillaId { get; set; }

        public List<int> FilterIds { get; set; }

        public List<int> BanderasId { get; set; }


        public override List<Expression<Func<HMinxtipo, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<HMinxtipo, object>>>
            {
                e=> e.HDetaminxtipo,
                e=> e.TipoHoraNavigation,
                e=> e.CodBanNavigation
            };
        }

        public override Expression<Func<HMinxtipo, bool>> GetFilterExpression()
        {

            var exp = base.GetFilterExpression();


            if (CodHfecha.HasValue)
            {
                exp = exp.And(e => e.CodHfecha == this.CodHfecha);
            }

            if (CodTdia.HasValue)
            {
                exp = exp.And(e => e.CodTdia == this.CodTdia);
            }

            if (!string.IsNullOrEmpty(TipoHora))
            {
                exp = exp.And(e => e.TipoHora == this.TipoHora);
            }

            if (CodBan.HasValue)
            {
                exp = exp.And(e => e.CodBan == this.CodBan);
            }

            if (FilterIds != null && FilterIds.Any())
            {
                exp = exp.And(e => FilterIds.Any(c => c == e.Id));
            }

            return exp;
        }

    }



    public class CopiarHMinxtipoInput
    {
        public CopiarHMinxtipoInput()
        {
            this.BanderasId = new List<int>();
        }

        #region Origen

        public int? CodHfechaOrigen { get; set; }
        public int? CodTdiaOrigen { get; set; }
        public string TipoHoraOrigen { get; set; }

        #endregion

        #region Destino
        public int? CodHfechaDestino { get; set; }
        public int? CodTdiaDestino { get; set; }
        public string TipoHoraDestino { get; set; }
        #endregion


        public List<int> BanderasId { get; set; }

    }

}
