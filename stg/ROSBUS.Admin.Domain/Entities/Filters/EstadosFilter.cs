using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class EstadosFilter : FilterPagedListFullAudited<SinEstados, int>
    {
        public string Descripcion { get; set; }

        public bool? Anulado { get; set; }

        public int? EstadoId { get; set; }

        public int? OrdenCambioEstado { get; set; }

        public override Expression<Func<SinEstados, bool>> GetFilterExpression()
        {
            Expression<Func<SinEstados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.Descripcion))
            {
                Expression<Func<SinEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Descripcion == Descripcion;
                Exp = Exp.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                Expression<Func<SinEstados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Anulado == this.Anulado;
                Exp = Exp.And(filterTextExp);
            }

            if (OrdenCambioEstado.HasValue)
            {
                Expression<Func<SinEstados, bool>> filterTextExp = e => ((e.OrdenCambioEstado > this.OrdenCambioEstado) || e.OrdenCambioEstado == null);
                Exp = Exp.And(filterTextExp);
            }

            if (EstadoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.EstadoId.Value);
            }


            return Exp;
        }

        //public override Func<SinEstados, ItemDto<int>> GetItmDTO()
        //{
        //    return e => new EstadoItemDto(e.Id, e.Descripcion, e.OrdenCambioEstado ?? e.OrdenCambioEstado.Value);
        //}

    }




    public class EstadoItemDto : ItemDto
    {

        public EstadoItemDto()
        {
        }

        public EstadoItemDto(int value, string displayText, int OrdenCambioEstado, bool isSelected = false) : base()
        {
            this.Id = value;
            this.Description = displayText;
            this.IsSelected = isSelected;
            this.OrdenCambioEstado = OrdenCambioEstado;
        }
        public int OrdenCambioEstado { get; set; }
    }

}
