using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Extensions;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.Admin.AppService.Model
{
    public class SiniestrosDto : EntityDto<int>
    {
        public SiniestrosDto()
        {
            this.SiniestrosConsecuencias = new List<SiniestrosConsecuenciasDto>();
           // this.Involucrados = new List<InvolucradosDto>();
        }

        [Required]
        public int SucursalId { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public TimeSpan Hora { get; set; }
        public string Dia { get; set; }
        public string CocheId { get; set; }
        [Required]
        public string EmpPract { get; set; }
        public int? ConductorId { get; set; }
        public int? PracticanteId { get; set; }
        [Required]
        public string Lugar { get; set; }
        [Required]
        public string Comentario { get; set; }
        public string CocheInterno { get; set; }
        public string CostoReparacion { get; set; }
        [Required]
        public bool? Responsable { get; set; }
        [Required]
        public bool? Descargo { get; set; }
        [Required]
        public DateTime FechaDenuncia { get; set; }
        public decimal? ConductorEmpresaId { get; set; }
        [Required]
        public decimal EmpresaId { get; set; }
        [Required]
        public decimal? CocheLineaId { get; set; }
        [Required]
        public string NroSiniestro { get; set; }
        public int? CausaId { get; set; }
        public int? SubCausaId { get; set; }
        public bool? GenerarInforme { get; set; }
        public string CodInforme { get; set; }
        public string ObsDanios { get; set; }
        public string ObsInterna { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string Localidad { get; set; }
        public int? FactoresId { get; set; }
        public int? ConductaId { get; set; }
        public int? SeguroId { get; set; }
        public string NroSiniestroSeguro { get; set; }
        public string CocheDominio { get; set; }
        public int? CocheFicha { get; set; }

        public string ConductorLegajo { get; set; }
        //public int? BandaSiniestralId { get; set; }
        public int? TipoDanioId { get; set; }
        [Required]
        public int? TipoColisionId { get; set; }
        public string NombreConductor { get; set; }
        public string DniConductor { get; set; }

        public bool? InformaTaller { get; set; }
        public int? SancionSugeridaId { get; set; }
        public string CroquiBase64 { get; set; }
        public string CuilEmpleado { get; set; }
        public bool Anulado { get; set; }
        public int? CroquiId { get; set; }
        public string SancionSugeridaDesc { get; set; }
        public SancionSugeridaDto SancionSugerida { get; set; }
        public CausasDto Causa { get; set; }
        public SubCausasDto SubCausa { get; set; }
        // public EmpleadosDto Empleado { get; set; }
        public LineaDto CocheLinea { get; set; }
        public List<SiniestrosConsecuenciasDto> SiniestrosConsecuencias { get; set; }
        public EmpresaDto Empresa { get; set; }
        public EmpresaDto ConductorEmpresa { get; set; }
        public PracticantesDto Practicante { get; set; }
        public string DescCombo { get; set; }
        public override string Description => this.DescCombo;
        public sucursalesDto Sucursal { get; set; }
        public ItemDto<string> selectCoches { get; set; }
        public ItemDto<string> selectPracticantes { get; set; }
        public string DescripcionSucursal { get; set; }
        public string DescripcionLinea { get; set; }

        //public ICollection<InvolucradosDto> Involucrados { get; set; }
        public GrillaConductor GrillaConductor
        {
            get
            {
                GrillaConductor _GrillaConductor = new GrillaConductor();

                if(this.ConductorId.HasValue)
                {
                    _GrillaConductor.NombreConductor = this.NombreConductor;
                    _GrillaConductor.Legajo = (this.ConductorLegajo) ?? "";
                    _GrillaConductor.NroDoc = "D.N.I: " + ((this.DniConductor) ?? "");
                }
                else if (this.PracticanteId.HasValue)
                {
                    _GrillaConductor.NombreConductor = (this.Practicante?.ApellidoNombre) ?? "";
                    _GrillaConductor.Legajo = (this.ConductorLegajo) ?? "";
                    _GrillaConductor.NroDoc = ((this.Practicante?.TipoDoc?.Descripcion) ?? "") + " " + (this.Practicante?.NroDoc) ?? "";
                }
                else
                {
                    _GrillaConductor.NombreConductor = " ";
                    _GrillaConductor.Legajo = " ";
                    _GrillaConductor.NroDoc = " ";
                }               

                return _GrillaConductor;

            }
            
        }

        public string CreatedUserName { get; set; }
        public string Direccion { get; set; }

        public string nro_informe { get; set; }

        public string ApellidoInvolucrado { get; set; }
        public string PrimerConsecuencia { get; set; }
        public string EstadoDeReclamos { get; set; }

    }


    public class GrillaConductor
    {
        public string NombreConductor { get; set; }
        public string Legajo { get; set; }



        public string NroDoc { get; set; }
    }
}
