using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class SiniestrosFilter : FilterPagedListFullAudited<SinSiniestros, int>
    {
        public string Descripcion { get; set; }
        public string NroSiniestro { get; set; }
        public Boolean? Anulado { get; set; }
        public int? SucursalId { get; set; }

        public DateTime? FechaSiniestroDesde { get; set; }
        public DateTime? FechaSiniestroHasta { get; set; }
        public DateTime? FechaDenunciaDesde { get; set; }
        public DateTime? FechaDenunciaHasta { get; set; }

        public int? Ficha { get; set; }

        public int? EmpresaId { get; set; }

        public ItemDto Linea { get; set; }
        public string Interno { get; set; }
        public string Dominio { get; set; }

        public int? PracticanteId { get; set; }
        public int? ConductorId { get; set; }

        public int? DescargoConId { get; set; }
        public int? ResponsableConId { get; set; }

        public string Ubicacion { get; set; }
        public ItemDto<int> selectEmpleados { get; set; }

        public ItemDto<int> selectPracticantes { get; set; }

        public string DominioInvolucrado { get; set; }
        public string NroDocInvolucrado { get; set; }
        public string ApellidoInvolucrado { get; set; }

        public Boolean ReclamosSearch { get; set; }
        public int? EstadoReclamo { get; set; }

        public int? SiniestroId { get; set; }

        public int? CausaId { get; set; }
        public List<int> Consecuencias { get; set; }

        public List<int> SubEstadoReclamo { get; set; }
        public bool? CheckAllConsecuencias { get; set; }
        public override Expression<Func<SinSiniestros, bool>> GetFilterExpression()
        {
            Expression<Func<SinSiniestros, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp;

                if (this.FilterText.Length >= 10)
                {
                    filterTextExp = e => e.NroSiniestro.Trim().Contains(this.FilterText)
                    || e.Sucursal.DscSucursal.Contains(this.FilterText)
                    || e.Fecha.ToString("dd/MM/yyyy") == this.FilterText
                    || e.Hora.ToString(@"hh\:mm") == this.FilterText;
                }
                else
                {
                    filterTextExp = e => e.NroSiniestro.Trim().Contains(this.FilterText) || e.Sucursal.DscSucursal.Contains(this.FilterText);
                }

                Exp = Exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.NroSiniestro))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.NroSiniestro.Trim() == this.NroSiniestro;
                Exp = Exp.And(filterTextExp);
            }
            if (Anulado.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.Anulado == this.Anulado.Value;
                Exp = Exp.And(filterTextExp);
            }
            if (SucursalId.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.SucursalId == this.SucursalId.Value;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaSiniestroDesde.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Fecha >= this.FechaSiniestroDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaSiniestroHasta.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Fecha <= this.FechaSiniestroHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaDenunciaDesde.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.FechaDenuncia >= this.FechaDenunciaDesde.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (FechaDenunciaHasta.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.FechaDenuncia <= this.FechaDenunciaHasta.Value.Date;
                Exp = Exp.And(filterTextExp);
            }
            if (EmpresaId.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.EmpresaId == this.EmpresaId.Value;
                Exp = Exp.And(filterTextExp);
            }
            if (CausaId.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.CausaId == this.CausaId.Value;
                Exp = Exp.And(filterTextExp);
            }
            if (ConductorId.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.ConductorId == this.ConductorId.Value;
                Exp = Exp.And(filterTextExp);
            }
            if (Ficha.HasValue)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.CocheFicha == this.Ficha;
                Exp = Exp.And(filterTextExp);
            }
            if (Linea != null)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.CocheLineaId == this.Linea.Id;
                Exp = Exp.And(filterTextExp);
            }
            if (!String.IsNullOrEmpty(this.Interno))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.CocheInterno.Contains(this.Interno);
                Exp = Exp.And(filterTextExp);
            }
            if (!String.IsNullOrEmpty(this.Dominio))
            {

                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.CocheDominio.Contains(this.Dominio.Replace(" ", String.Empty));
                Exp = Exp.And(filterTextExp);
            }
            if (selectPracticantes != null)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.PracticanteId == selectPracticantes.Id;
                Exp = Exp.And(filterTextExp);
            }
            if (selectEmpleados != null)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.ConductorId == selectEmpleados.Id;
                Exp = Exp.And(filterTextExp);
            }
            if (DescargoConId.HasValue)
            {
                if (DescargoConId.Value == 1)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    filterTextExp = e => e.Descargo == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (DescargoConId.Value == 2)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    filterTextExp = e => e.Descargo == false;
                    Exp = Exp.And(filterTextExp);
                }
            }
            if (ResponsableConId.HasValue)
            {
                if (ResponsableConId.Value == 1)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    filterTextExp = e => e.Responsable == true;
                    Exp = Exp.And(filterTextExp);
                }
                else if (ResponsableConId.Value == 2)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    filterTextExp = e => e.Responsable == false;
                    Exp = Exp.And(filterTextExp);
                }
            }
            if (!String.IsNullOrEmpty(this.Ubicacion))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.Lugar.Contains(this.Ubicacion);
                Exp = Exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.DominioInvolucrado))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.SinInvolucrados.Any(f => f.Vehiculo.Dominio.Contains(this.DominioInvolucrado.Replace(" ", String.Empty)) && f.IsDeleted == false);
                Exp = Exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.NroDocInvolucrado))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                filterTextExp = e => e.SinInvolucrados.Any(f => f.NroDoc.Contains(this.NroDocInvolucrado) && f.IsDeleted == false);
                Exp = Exp.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.ApellidoInvolucrado))
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.SinInvolucrados.Any(f => f.ApellidoNombre.ToLower().Trim().Contains(this.ApellidoInvolucrado.ToLower().Trim()) && f.IsDeleted == false);
                Exp = Exp.And(filterTextExp);
            }

            if (EstadoReclamo.HasValue && SubEstadoReclamo != null && SubEstadoReclamo.Count >= 1)
            {
                Exp = Exp.And(e => e.Reclamos.Any(y => y.EstadoId == this.EstadoReclamo.Value && this.SubEstadoReclamo.Contains(y.SubEstadoId) && y.IsDeleted == false));
            }
            else
            {
                if (EstadoReclamo.HasValue)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    // filterTextExp = e => e.SinInvolucrados.Where(f => f.SinReclamos.Any(y => y.EstadoId == this.EstadoReclamo.Value));
                    filterTextExp = e => e.Reclamos.Any(y => y.EstadoId == this.EstadoReclamo.Value && y.IsDeleted == false);
                    Exp = Exp.And(filterTextExp);
                }
                if (SubEstadoReclamo != null && SubEstadoReclamo.Count >= 1)
                {
                    Expression<Func<SinSiniestros, bool>> filterTextExp = e => true;
                    filterTextExp = e => e.Reclamos.Any(y => this.SubEstadoReclamo.Contains(y.SubEstadoId) && y.IsDeleted == false);
                    Exp = Exp.And(filterTextExp);
                }
            }


            if (Consecuencias != null)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExpConsec;

                if (!CheckAllConsecuencias.HasValue || (CheckAllConsecuencias.HasValue && CheckAllConsecuencias.Value))
                {
                    if (Consecuencias.Count >= 1)
                    {
                        foreach (var Consecuencia in Consecuencias)
                        {
                            Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.SinSiniestrosConsecuencias.Any(f => f.ConsecuenciaId == Consecuencia && f.IsDeleted == false);
                            Exp = Exp.And(filterTextExp);
                        }

                        Expression<Func<SinSiniestros, bool>> filterTextOb = e => e.SinSiniestrosConsecuencias.Where(f => f.IsDeleted == false).Count() == Consecuencias.Count;
                        Exp = Exp.And(filterTextOb);

                    }
                }
                else
                {

                    if (Consecuencias.Count > 0)
                    {
                        filterTextExpConsec = e => e.SinSiniestrosConsecuencias.Any(f => f.ConsecuenciaId == Consecuencias[0] && f.IsDeleted == false);
                        for (int i = 1; i < Consecuencias.Count; i++)
                        {
                            var value = Consecuencias[i];
                            Expression<Func<SinSiniestros, bool>> filterTextExp = e => e.SinSiniestrosConsecuencias.Any(f => f.ConsecuenciaId == value && f.IsDeleted == false);
                            filterTextExpConsec = filterTextExpConsec.Or(filterTextExp);
                        }
                        Exp = Exp.And(filterTextExpConsec);
                    }



                }
            }

            if (ReclamosSearch)
            {
                Expression<Func<SinSiniestros, bool>> filterTextExp = e => (e.SinInvolucrados.Count > 0);
                Exp = Exp.And(filterTextExp);
            }

            if (SiniestroId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.SiniestroId.Value);
            }
            return Exp;
        }


        public override List<Expression<Func<SinSiniestros, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinSiniestros, object>>>
            {
                e=> e.Sucursal,
                e=> e.Empresa,
                e=> e.ConductorEmpresa,
                e=> e.CocheLinea,
                e=> e.SinSiniestrosConsecuencias,
                e=> e.Practicante,
                e=> e.Practicante.TipoDoc,
                e=> e.SubCausa,
                e=> e.SancionSugerida
            };
        }


        public override Func<SinSiniestros, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, e.getDescription());
        }

    }
}
