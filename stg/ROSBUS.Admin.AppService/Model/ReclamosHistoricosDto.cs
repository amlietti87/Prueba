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
    public class ReclamosHistoricosDto : EntityDto<int>
    {
        public int ReclamoId { get; set; }
        public int? InvolucradoId { get; set; }
        public int? TipoReclamoId { get; set; }
        public DateTime Fecha { get; set; }
        public int EstadoId { get; set; }
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
        public int? EmpleadoId { get; set; }
        public decimal? EmpresaId { get; set; }
        public int? SucursalId { get; set; }
        public DateTime? EmpleadoFechaIngreso { get; set; }
        public string EmpleadoLegajo { get; set; }
        public DateTime? EmpleadoAntiguedad { get; set; }
        public decimal? EmpleadoEmpresaId { get; set; }
        public string EmpleadoArea { get; set; }
        public string Hechos { get; set; }
        public int? RubroSalarialId { get; set; }
        public int? TipoAcuerdoId { get; set; }
        public int? DenunciaId { get; set; }
        public string NombreEmpleado { get; set; }
        public bool JudicialSelected { get; set; }
        public string CreatedUserName { get; set; }
        public ItemDto<int> selectedEmpleado => getItemEmpleado();
        public ItemDto<int> selectedSiniestro { get; set; }
        public ItemDto<int> selectedDenuncia { get; set; }
        public AbogadosDto Abogado { get; set; }
        public AbogadosDto AbogadoEmpresa { get; set; }
        public EstadosDto Estado { get; set; }
        public InvolucradosDto Involucrado { get; set; }
        public JuzgadosDto Juzgado { get; set; }
        public SubEstadosDto SubEstado { get; set; }

        public TiposReclamoDto TipoReclamo { get; set; }
        public ReclamosDto Reclamo { get; set; }
        public override string Description => string.Empty;
        public DateTime CreatedDate { get; set; }

        public ItemDto<int> getItemEmpleado()
        {
            if (EmpleadoId.HasValue)
            {
                return new ItemDto<int>(EmpleadoId.Value, string.Format("{0} - {1}", NombreEmpleado, EmpleadoLegajo));
            }
            else
            {
                return null;
            }
        }
    }
}
