using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HChoxserDto :EntityDto<int>
    {
        
        public override string Description => "";


        public int CodUni { get; set; }

        public int CodServicio
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }

        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public string CodEmp { get; set; }
        public int? TipoMultiple { get; set; }
    }

    public class HorarioDuracion
    {
        public HChoxserExtendedDto Duracion { get; set; }
        public HHorariosConfiDto Horario { get; set; }

    }
}
