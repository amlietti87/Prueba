using ROSBUS.Admin.Domain.Model;
using ROSBUS.ART.AppService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ReclamosDto :EntityDto<int>
    {
        public ReclamosDto()
        {
            this.ReclamoCuotas = new List<ReclamoCuotasDto>();
        }
        public int? InvolucradoId { get; set; }
        [Required]
        public int? TipoReclamoId { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public int EstadoId { get; set; }
        [Required]
        public int SubEstadoId { get; set; }
        public decimal? MontoDemandado { get; set; }
        public DateTime? FechaOfrecimiento { get; set; }
        public decimal? MontoOfrecido { get; set; }
        public decimal? MontoReconsideracion { get; set; }
        public bool? Cuotas { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal? MontoPagado { get; set; }
        public decimal? MontoFranquicia { get; set; }
        public int? AbogadoId { get; set; }
        public int? CausaId { get; set; }
        public decimal? MontoHonorariosAbogado { get; set; }
        public decimal? MontoHonorariosMediador { get; set; }
        public decimal? MontoHonorariosPerito { get; set; }
        public decimal? MontoTasasJudiciales { get; set; }
        public bool? JuntaMedica { get; set; }
        public decimal? PorcentajeIncapacidad { get; set; }
        public string Observaciones { get; set; }
        public string ObsConvenioExtrajudicial { get; set; }
        public string Autos { get; set; }
        public string NroExpediente { get; set; }
        public int? JuzgadoId { get; set; }
        public int? AbogadoEmpresaId { get; set; }
        public int? SiniestroId { get; set; }
        public int? DenunciaId { get; set; }
        public int? EmpleadoId { get; set; }
        public decimal? EmpresaId { get; set; }
        public int? SucursalId { get; set; }
        public string MotivoAnulado { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public decimal? EmpleadoEmpresaId { get; set; }
        public string EmpleadoArea { get; set; }
        public string Hechos { get; set; }
        public int? RubroSalarialId { get; set; }
        public int? TipoAcuerdoId { get; set; }

        public bool JudicialSelected { get; set; }
        public string NombreEmpleado { get; set; }
        public string CreatedUserName { get; set; }

        public string EmpleadoGrilla { get; set; }

        public string InvolucradoGrilla
        {
            get
            {
                if (Involucrado != null)
                { 
                    return "<b>" + Involucrado?.NroInvolucrado + "</b>" + " - " + Involucrado?.ApellidoNombre + " - " + Involucrado?.TipoInvolucradoNombre;
                }
                else
                {
                    return null;
                }
            }
        }
        public ItemDto<int> selectedSiniestro { get; set; }
        public ItemDto<int> selectedEmpleado => getItemEmpleado();
        public Boolean Anulado { get; set; }
        public AbogadosDto Abogado { get; set; }
        public AbogadosDto AbogadoEmpresa { get; set; }
        public EstadosDto Estado { get; set; }
        public InvolucradosDto Involucrado { get; set; }
        public JuzgadosDto Juzgado { get; set; }
        public SubEstadosDto SubEstado { get; set; }

        public TiposReclamoDto TipoReclamo { get; set; }
        public sucursalesDto Sucursal { get; set; }
        public EmpresaDto Empresa { get; set; }
        // public ICollection<SinReclamoAdjuntos> SinReclamoAdjuntos { get; set; }
        public ICollection<ReclamoCuotasDto> ReclamoCuotas { get; set; }
        public string ReclamoHistorico { get; set; }
        public override string Description => string.Empty;

        public bool EnableADet
        {
            get
            {
                if (Involucrado != null && Involucrado.TipoInvolucradoNombre?.Trim().ToLower() == "a determinar" && String.IsNullOrEmpty(ReclamoHistorico))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public ItemDto<int> getItemEmpleado()
        {
            if (EmpleadoId.HasValue) { 
            return new ItemDto<int>(EmpleadoId.Value, string.Format("{0} - {1}", NombreEmpleado, EmpleadoLegajo));
            }
            else
            {
                return null;
            }
        }
    }
}
