using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{

    public class ReclamosFilter : FilterPagedListFullAudited<SinReclamos, int>
    {
        public int? SiniestroId { get; set; }
        public int? EstadoId { get; set; }
        public int? SubEstadoId { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string Documento { get; set; }
        public string Apellido { get; set; }
        public string Dominio { get; set; }
        public int? InvolucradoId { get; set; }
        public int? TipoInvolucradoId { get; set; }
        public int? SucursalId { get; set; }
        public int? EmpresaId { get; set; }

        public int? AnuladoCombo { get; set; }
        public ItemDto<int> selectEmpleados { get; set; }
        public DateTime? FechaReclamoDesde { get; set; }
        public DateTime? FechaReclamoHasta { get; set; }
        public int? TipoReclamoId { get; set; }
        public string NroDenuncia { get; set; }
        public string NroSiniestro { get; set; }

        public List<int> SubEstadoReclamo { get; set; }
        public DateTime? FechaPagoDesde { get; set; }
        public DateTime? FechaPagoHasta { get; set; }
        public int? AbogadoId { get; set; }

        public bool? IsDeleted { get; set; }
        public override Expression<Func<SinReclamos, bool>> GetFilterExpression()
        {
            Expression<Func<SinReclamos, bool>> Exp = base.GetFilterExpression();


            if (SiniestroId.HasValue)
            {
                Exp = Exp.And(e => e.SiniestroId == this.SiniestroId);
                Exp = Exp.And(e => e.Involucrado.IsDeleted == false);
            }
            if (EstadoId.HasValue && SubEstadoReclamo != null && SubEstadoReclamo.Count >= 1)
            {
                Exp = Exp.And(y => y.EstadoId == this.EstadoId.Value && this.SubEstadoReclamo.Contains(y.SubEstadoId) && y.IsDeleted == false);
            }
            else
            {
                if (EstadoId.HasValue)
                {
                    Expression<Func<SinReclamos, bool>> filterTextExp = y => y.EstadoId == this.EstadoId.Value && y.IsDeleted == false;
                    Exp = Exp.And(filterTextExp);
                }
                if (SubEstadoReclamo != null && SubEstadoReclamo.Count >= 1)
                {
                    Expression<Func<SinReclamos, bool>> filterTextExp = y => this.SubEstadoReclamo.Contains(y.SubEstadoId) && y.IsDeleted == false;
                    Exp = Exp.And(filterTextExp);
                }
            }
            if (TipoDocumentoId.HasValue)
            {
                Exp = Exp.And(e => e.Involucrado.TipoDocId == this.TipoDocumentoId);
            }
            if (!String.IsNullOrWhiteSpace(Documento))
            {
                Exp = Exp.And(e => e.Involucrado.NroDoc.Contains(Documento));
            }
            if (!String.IsNullOrWhiteSpace(Apellido))
            {
                Exp = Exp.And(e => e.Involucrado.ApellidoNombre.Contains(Apellido));
            }
            if (InvolucradoId.HasValue)
            {
                Exp = Exp.And(e => e.InvolucradoId == this.InvolucradoId);
            }
            if (SucursalId.HasValue)
            {
                Exp = Exp.And(e => e.SucursalId == this.SucursalId);
            }
            if (EmpresaId.HasValue)
            {
                Exp = Exp.And(e => e.EmpresaId == this.EmpresaId);
            }
            if (EmpresaId.HasValue)
            {
                Exp = Exp.And(e => e.EmpresaId == this.EmpresaId);
            }
            if (selectEmpleados != null)
            {
                Expression<Func<SinReclamos, bool>> filterTextExp = e => e.EmpleadoId == selectEmpleados.Id;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaReclamoDesde.HasValue)
            {
                Exp = Exp.And(e => e.Fecha.Date >= this.FechaReclamoDesde.Value.Date);
            }
            if (FechaReclamoHasta.HasValue)
            {
                Exp = Exp.And(e => e.Fecha.Date <= this.FechaReclamoHasta.Value.Date);
            }
            if (TipoReclamoId.HasValue)
            {
                Exp = Exp.And(e => e.TipoReclamoId == this.TipoReclamoId);
            }

            if (!String.IsNullOrWhiteSpace(Dominio))
            {
                Exp = Exp.And(e => e.Involucrado.Vehiculo.Dominio.Contains(this.Dominio));
            }

            if (!String.IsNullOrWhiteSpace(NroDenuncia))
            {
                Exp = Exp.And(e => e.Denuncia.NroDenuncia.Contains(this.NroDenuncia));
            }

            if (!String.IsNullOrWhiteSpace(NroSiniestro))
            {
                Exp = Exp.And(e => e.Siniestro.NroSiniestro.Contains(this.NroSiniestro));
            }

            if (FechaPagoDesde.HasValue)
            {
                Exp = Exp.And(e => e.FechaPago >= this.FechaPagoDesde);
            }
            if (FechaPagoHasta.HasValue)
            {
                Exp = Exp.And(e => e.FechaPago <= this.FechaPagoHasta);
            }

            if (AbogadoId.HasValue)
            {
                Exp = Exp.And(e => e.AbogadoId == this.AbogadoId);
            }

            if (AnuladoCombo.HasValue)
            {
                if (AnuladoCombo.Value == 1)
                {
                    Expression<Func<SinReclamos, bool>> filterTextExp = e => e.Anulado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (AnuladoCombo.Value == 2)
                {
                    Expression<Func<SinReclamos, bool>> filterTextExp = e => e.Anulado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (IsDeleted.HasValue)
            {
                Exp = Exp.And(e => e.IsDeleted == this.IsDeleted);
            }
            if (TipoInvolucradoId.HasValue)
            {
                Exp = Exp.And(e => e.Involucrado.TipoInvolucradoId == this.TipoInvolucradoId);
            }

            return Exp;
        }

        public override List<Expression<Func<SinReclamos, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinReclamos, object>>>() {
                e => e.Estado,
                e => e.Involucrado,
                e => e.SubEstado,
                e => e.ReclamoCuotas,
                e => e.TipoReclamo,
                e => e.CreatedUser,
                e => e.Sucursal,
                e => e.Empresa
            };
        }

        public override Func<SinReclamos, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, e.Involucrado.ApellidoNombre);
        }
    }

    public class ExcelReclamosFilter
    {
        public string Ids { get; set; }
    }

}
