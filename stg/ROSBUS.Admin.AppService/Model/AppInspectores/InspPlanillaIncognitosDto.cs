using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class InspPlanillaIncognitosDto :EntityDto<int>
    {

        public InspPlanillaIncognitosDto()
        {
            InspPlanillaIncognitosDetalle = new List<InspPlanillaIncognitosDetalleDto>();
        }
        [Required]
        public DateTime Fecha { get; set; }


        public int? SucursalId { get; set; }

        [Required]
        public DateTime HoraAscenso { get; set; }
        public DateTime? HoraDescenso { get; set; }

        [Required]
        public string CocheId { get; set; }
        
        [Required]
        public int CocheFicha { get; set; }
        
        [Required]
        public string CocheInterno { get; set; }

        [Required]
        public decimal Tarifa { get; set; }

        [Required]
        public virtual List<InspPlanillaIncognitosDetalleDto> InspPlanillaIncognitosDetalle { get; set; }
        public override string Description => "";
    }
}
