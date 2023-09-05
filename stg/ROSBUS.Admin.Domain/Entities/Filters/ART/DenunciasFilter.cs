using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Extensions;
using System.Linq;

namespace ROSBUS.Admin.Domain.Entities.Filters.ART
{
    public class DenunciasFilter : FilterPagedListFullAudited<ArtDenuncias, int>
    {

        public string NroDenuncia { get; set; }
        public int? SucursalId { get; set; }
        public int? EmpresaId { get; set; }
        public int? EmpleadoId { get; set; }
        public ItemDto<int> selectEmpleados { get; set; }
        public ItemDto<int> selectDenuncia { get; set; }
        public DateTime? FechaDenunciaDesde { get; set; }
        public DateTime? FechaDenunciaHasta { get; set; }

        public DateTime? FechaOcurrenciaDesde { get; set; }
        public DateTime? FechaOcurrenciaHasta { get; set; }

        public int? EstadoDenuncia { get; set; }

        public int? ContingenciaId { get; set; }
        public int? PatologiaId { get; set; }
        public int? PrestadorMedicoId { get; set; }
        public int? BajaServicio { get; set; }

        public int? TratamientoId { get; set; }

        public DateTime? FechaUltimoControlDesde { get; set; }
        public DateTime? FechaUltimoControlHasta { get; set; }
        
        public DateTime? FechaProxConsultaDesde { get; set; }
        public DateTime? FechaProxConsultaHasta { get; set; }
        public DateTime? FechaAudienciaDesde { get; set; }
        public DateTime? FechaAudienciaHasta { get; set; }

        public int? MotivoAudienciaId { get; set; }
        public int? AltaLaboral { get; set; }
        public int? AltaMedica { get; set; }
        public int? TieneReingresos { get; set; }
        public int? DenunciaIdOrigen { get; set; }
        public int? Siniestro { get; set; }
        public int? Juicio { get; set; }
        public int? Anulado { get; set; }
        public int? NotId { get; set; }

        public string PlanillaId { get; set; }

        public override Expression<Func<ArtDenuncias, bool>> GetFilterExpression()
        {
            Expression<Func<ArtDenuncias, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp;

                if (this.FilterText.Length >= 10)
                {
                    filterTextExp = e => e.NroDenuncia.Trim().Contains(this.FilterText)
                    || e.PrestadorMedico.Descripcion.Contains(this.FilterText)
                    || e.FechaOcurrencia.ToString("dd/MM/yyyy") == this.FilterText
                    || e.FechaRecepcionDenuncia.ToString("dd/MM/yyyy") == this.FilterText;
                }
                else
                {
                    filterTextExp = e => e.NroDenuncia.Trim().Contains(this.FilterText)
                    || e.PrestadorMedico.Descripcion.Contains(this.FilterText);
                }

                Exp = Exp.And(filterTextExp);
            }



            if (!String.IsNullOrEmpty(this.NroDenuncia))
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.NroDenuncia.Contains(this.NroDenuncia);
                Exp = Exp.And(filterTextExp);
            }

            if (SucursalId.HasValue)
            {
                Exp = Exp.And(e => e.SucursalId == this.SucursalId.Value);
            }

            if (EmpresaId.HasValue)
            {
                Exp = Exp.And(e => e.EmpresaId == this.EmpresaId.Value);
            }

            if (selectEmpleados != null)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e =>  e.EmpleadoId == selectEmpleados.Id;
                Exp = Exp.And(filterTextExp);
            }

            if (FechaDenunciaDesde.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaRecepcionDenuncia.Date >= FechaDenunciaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaDenunciaHasta.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaRecepcionDenuncia.Date <= FechaDenunciaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }

            if (FechaOcurrenciaDesde.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaOcurrencia.Date >= FechaOcurrenciaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaOcurrenciaHasta.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaOcurrencia.Date <= FechaOcurrenciaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (ContingenciaId.HasValue)
            {
                Exp = Exp.And(e => e.ContingenciaId == this.ContingenciaId.Value);
            }
            if (PatologiaId.HasValue)
            {
                Exp = Exp.And(e => e.PatologiaId == this.PatologiaId.Value);
            }
            if (PrestadorMedicoId.HasValue)
            {
                Exp = Exp.And(e => e.PrestadorMedicoId == this.PrestadorMedicoId.Value);
            }
            if (EmpleadoId.HasValue)
            {
                Exp = Exp.And(e => e.EmpleadoId == this.EmpleadoId.Value);
            }

            if (BajaServicio.HasValue)
            {
                if (BajaServicio.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e =>  e.BajaServicio == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (BajaServicio.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.BajaServicio == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (TratamientoId.HasValue)
            {
                Exp = Exp.And(e => e.TratamientoId == this.TratamientoId.Value);
            }

            if (FechaUltimoControlDesde.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaUltimoControl.Value.Date >= FechaUltimoControlDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaUltimoControlHasta.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaUltimoControl.Value.Date <= FechaUltimoControlHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }

            if (FechaProxConsultaDesde.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaProximaConsulta.Value.Date >= FechaProxConsultaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaProxConsultaHasta.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaProximaConsulta.Value.Date <= FechaProxConsultaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }

            if (FechaAudienciaDesde.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaAudienciaMedica.Value.Date >= FechaAudienciaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaAudienciaHasta.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.FechaAudienciaMedica.Value.Date <= FechaAudienciaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }

            if (MotivoAudienciaId.HasValue)
            {
                Exp = Exp.And(e => e.MotivoAudienciaId == this.MotivoAudienciaId.Value);
            }

            if (Siniestro.HasValue)
            {
                Exp = Exp.And(e => e.SiniestroId == this.Siniestro.Value);
            }

            if (AltaLaboral.HasValue)
            {
                if (AltaLaboral.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.AltaLaboral == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (AltaLaboral.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.AltaLaboral == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (AltaMedica.HasValue)
            {
                if (AltaMedica.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.AltaMedica == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (AltaMedica.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.AltaMedica == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (TieneReingresos.HasValue)
            {
                if (TieneReingresos.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.TieneReingresos == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (TieneReingresos.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.TieneReingresos == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            //if (!String.IsNullOrEmpty(this.SiniestroId))
            //{
            //    Exp = Exp.And(e => e.Siniestro.NroSiniestro == this.Siniestro);
            //}

            if (Juicio.HasValue)
            {
                if (Juicio.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Juicio == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (Juicio.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Juicio == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (Anulado.HasValue)
            {
                if (Anulado.Value == 1)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Anulado == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (Anulado.Value == 2)
                {
                    Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Anulado == false;
                    Exp = Exp.And(filterTextExp);
                }
            }

            if (NotId.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Id != NotId;
                Exp = Exp.And(filterTextExp);
            }

            if (selectDenuncia != null)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.Id == selectDenuncia.Id;
                Exp = Exp.Or(filterTextExp);
            }

            if (EstadoDenuncia.HasValue)
            {
                Expression<Func<ArtDenuncias, bool>> filterTextExp = e => e.EstadoId == EstadoDenuncia;
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }


        public override List<Expression<Func<ArtDenuncias, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<ArtDenuncias, object>>>
            {
                e=> e.Sucursal,
                e=> e.Empresa,
                e=> e.Contingencia,
                e=> e.Patologia,
                e=> e.PrestadorMedico,
                e=> e.Tratamiento,
                e=> e.MotivoAudiencia,
                e=> e.ArtDenunciasNotificaciones,
                e=> e.CreatedUser
            };
        }
        public override Func<ArtDenuncias, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, e.getDescription());
        }
    }

    public class ExcelDenunciasFilter
    {
        public string Ids { get; set; }
    }
}
