using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Extensions;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class FdDocumentosProcesadosFilter : FilterPagedListBase<FdDocumentosProcesados, long>
    {
        public int? SucursalId { get; set; }

        public int? EmpresaId { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public int? TipoDocumentoId { get; set; }
        public ItemDto EmpleadoId { get; set; }

        public int? EstadoId { get; set; }
        public int? Rechazado { get; set; }

        public int? Cerrado { get; set; }

        public bool? EsEmpleador { get; set; }

        public override Expression<Func<FdDocumentosProcesados, bool>> GetFilterExpression()
        {
            Expression<Func<FdDocumentosProcesados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.TipoDocumento.Descripcion.Contains(this.FilterText);
                Exp = Exp.And(filterTextExp);
            }


            if (SucursalId.HasValue)
            {
                Exp = Exp.And(e => e.SucursalEmpleadoId == this.SucursalId.Value);
            }

            if (EmpresaId.HasValue)
            {
                Exp = Exp.And(e => e.EmpresaEmpleadoId == this.EmpresaId.Value);
            }

            if (FechaDesde.HasValue)
            {
                Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Fecha.Date >= FechaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaHasta.HasValue)
            {
                Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Fecha.Date <= FechaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }

            if (TipoDocumentoId.HasValue)
            {
                Exp = Exp.And(e => e.TipoDocumentoId == this.TipoDocumentoId.Value);
            }

            if (EmpleadoId != null)
            {
                Exp = Exp.And(e => e.EmpleadoId == this.EmpleadoId.Id);
            }

            if (EstadoId.HasValue)
            {
                Exp = Exp.And(e => e.EstadoId == this.EstadoId.Value);
            }

            if (Rechazado.HasValue)
            {
                if (Rechazado.Value == 1)
                {
                    Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Rechazado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (Rechazado.Value == 2)
                {
                    Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Rechazado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (Cerrado.HasValue)
            {
                if (Cerrado.Value == 1)
                {
                    Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Cerrado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (Cerrado.Value == 2)
                {
                    Expression<Func<FdDocumentosProcesados, bool>> filterTextExp = e => e.Cerrado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }
            if (this.EsEmpleador.HasValue && this.EsEmpleador.Value)
            {
                Expression<Func<FdDocumentosProcesados, bool>> filterFinal = e =>
                e.TipoDocumento.FdAcciones.Any(f =>
                (f.MostrarBdempleador == true && f.EstadoActualId == e.EstadoId)
                || (f.MostrarBdempleador == true && f.EstadoNuevoId == e.EstadoId && f.EsFin == true));
                Exp = Exp.And(filterFinal);
            }

            if (this.EsEmpleador.HasValue && this.EsEmpleador.Value == false)
            {
                Expression<Func<FdDocumentosProcesados, bool>> filterFinal = e =>
                e.TipoDocumento.FdAcciones.Any(f =>
                (f.MostrarBdempleado == true && f.EstadoActualId == e.EstadoId && e.Rechazado == false)
                || (f.MostrarBdempleado == true && f.EstadoNuevoId == e.EstadoId && f.EsFin == true));
                Exp = Exp.And(filterFinal);

                //Exp = Exp.And(e => e.EmpleadoId == 4);
            }
            return Exp;
        }

        public override List<Expression<Func<FdDocumentosProcesados, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.Sucursal,
                e => e.Empresa,
                e => e.TipoDocumento,
                e=> e.FdDocumentosProcesadosHistorico,
                e=> e.Estado
            };
        }

        public override List<Expression<Func<FdDocumentosProcesados, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.Sucursal,
                e => e.Empresa,
                e => e.TipoDocumento
            };
        }
    }
}
