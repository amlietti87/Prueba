using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class InspTareasRealizadasDto : EntityDto<int>
    {

        public InspTareasRealizadasDto()
        {
            TareasRealizadasDetalle = new List<InspTareasRealizadasDetalleDto>();
        }


        [Required(ErrorMessage = "El campo Fecha es Requerido")]
        public DateTime? Fecha { get; set; }

        
        public int? EmpleadoId { get; set; }

        public int? SucursalId { get; set; }

        [Required(ErrorMessage = "El campo Tarea es Requerido")]
        public int? TareaId { get; set; }



        public List<InspTareasRealizadasDetalleDto> TareasRealizadasDetalle { get; set; }

        public override string Description => "";
    }


    public class InspTareasRealizadasDetalleDto : EntityDto<int>
    {
        public int TareaRealizadaId { get; set; }
        public int TareaCampoId { get; set; }
        public string Valor { get; set; }


        public override string Description => "";
    }
}
