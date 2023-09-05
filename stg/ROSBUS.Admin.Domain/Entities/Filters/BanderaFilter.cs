using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Entities;
using System.Data;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class BanderaFilter : FilterPagedListFullAudited<HBanderas, int>
    {
        public BanderaFilter()
        {
            BanderasSeleccionadas = new List<int>();
        }

        public override Func<HBanderas, ItemDto<int>> GetItmDTO()
        {
            return e => new BanderaItemintDto(e.Id, e.AbrBan.Trim() ?? e.DesBan.Trim(), e.RamalColorId, e.RamalColor?.PlaLinea?.DesLin.TrimOrNull());
        }

        public override List<Expression<Func<HBanderas, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<HBanderas, object>>>
            {
                e=> e.RamalColor,
                e=> e.RamalColor.PlaLinea,
                 e=> e.RamalColor.PlaLinea.Sucursal,
                 e=> e.SentidoBandera
            };
        }

        public override List<Expression<Func<HBanderas, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<HBanderas, object>>>
            {
                // e=> e.RamalColor                
            };
        }

        public Boolean ValidarMediasVueltasIncompletas { get; set; }

        public List<int> BanderasSeleccionadas { get; set; }

        public Boolean ShowDecimalValues { get; set; }


        public string CodigoVarianteLinea { get; set; }


        public string AbrBan { get; set; }


        public int? SucursalId { get; set; }

        public int? RamaColorId { get; set; }

        public int? TipoBanderaId { get; set; }

        public decimal? LineaId { get; set; }

        public List<long?> RamalesID { get; set; }
        public int? BanderaRelacionadaID { get; set; }

        public int? SentidoBanderaId { get; set; }
        public DateTime? Fecha { get; set; }


        public int? cod_servicio { get; set; }

        public int? cod_band { get; set; }

        public string cod_Conductor { get; set; }

        public bool? Activo { get; set; }
        public bool Plano { get; set; }
        public bool NoDescartarPrimeryUltimoMV { get; set; }

        public int? CodHfecha { get; set; }
        public decimal? LineaIdRelacionadas { get; set; }
        public int? CodTdia { get; set; }
        public ItemDto Linea { get; set; }

        public override Expression<Func<HBanderas, bool>> GetFilterExpression()
        {
            Expression<Func<HBanderas, bool>> Exp = base.GetFilterExpression();


            if (this.BanderaRelacionadaID.HasValue)
            {
                //TODO: preguntar como filtramos esto
            }
            if (Activo.HasValue)
            {
                Exp = Exp.And(e => e.Activo == Activo.Value);
            }


            if (SentidoBanderaId.HasValue)
            {
                var sentidoBanderaId = this.SentidoBanderaId.Value;
                Exp = Exp.And(e => e.SentidoBanderaId == sentidoBanderaId);
            }

            if (SucursalId.HasValue)
            {
                Exp = Exp.And(e => e.SucursalId == SucursalId.Value);
            }

            if (LineaId.HasValue)
            {
                var lineaId = LineaId.Value;
                Exp = Exp.And(e => e.RamalColor.LineaId == lineaId);
            }

            if (RamaColorId.HasValue)
            {
                var ramaColorId = RamaColorId.Value;
                Exp = Exp.And(e => e.RamalColorId == ramaColorId);
            }

            if (this.RamalesID != null && this.RamalesID.Count > 0)
            {
                var ramalesID = this.RamalesID.ToList();
                Exp = Exp.And(e => ramalesID.Contains(e.RamalColorId));
            }


            if (!string.IsNullOrEmpty(this.CodigoVarianteLinea))
            {

                Exp = Exp.And(e => e.CodigoVarianteLinea.Contains(this.CodigoVarianteLinea));
            }

            if (TipoBanderaId.HasValue)
            {
                var tipoBanderaId = TipoBanderaId.Value;
                Exp = Exp.And(e => e.TipoBanderaId == tipoBanderaId);
            }


            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<HBanderas, bool>> filterTextExp = e => true;
                filterTextExp = e => e.AbrBan.Contains(this.FilterText) || e.DesBan.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }

            if (CodHfecha.HasValue && LineaIdRelacionadas.HasValue)
            {
                var _cod_hfecha = CodHfecha.Value;
                Exp = Exp.And(e => e.HBasec.Any(a => a.CodHfecha == _cod_hfecha || e.RamalColor.PlaLinea.PlaLineaLineaHoraria.Any(pa => pa.LineaId == LineaIdRelacionadas)));
            }
            else
            {
                if (CodHfecha.HasValue)
                {
                    var _cod_hfecha = CodHfecha.Value;
                    Exp = Exp.And(e => e.HBasec.Any(a => a.CodHfecha == _cod_hfecha));
                }

                if (LineaIdRelacionadas.HasValue)
                {
                    Exp = Exp.And(e => e.RamalColor.PlaLinea.PlaLineaLineaHoraria.Any(a => a.LineaId == LineaIdRelacionadas));
                }
            }


            if (Linea != null)
            {
                Exp = Exp.And(e => e.RamalColor.LineaId == Linea.Id);
            }

            return Exp;
        }

    }


    public class BanderaItemintDto : ItemDto
    {

        public BanderaItemintDto()
        {
        }

        public BanderaItemintDto(int value, string displayText, long? ramaColorId, string LineaNombre, bool isSelected = false) : base()
        {
            this.Id = value;
            this.Description = displayText;
            this.IsSelected = isSelected;
            this.RamaColorId = ramaColorId;
            this.LineaNombre = LineaNombre;
        }
        public long? RamaColorId { get; set; }

        public string LineaNombre { get; set; }
    }
}
