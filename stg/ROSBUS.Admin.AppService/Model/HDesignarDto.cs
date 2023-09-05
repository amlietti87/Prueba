using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HDesignarDto : EntityDto<int>
    {
        public DateTime Fecha { get; set; }
        public int CodServicio { get; set; }
        public int CodUni { get; set; }
        public DateTime Sale { get; set; }
        public DateTime? SaleOriginal { get; set; }
        public DateTime Llega { get; set; }
        public DateTime? LlegaOriginal { get; set; }
        public string CodEmp { get; set; }
        public DateTime? HorasModificadas { get; set; }
        public DateTime? HoraDesc { get; set; }
        public string TipoServ { get; set; }
        public string CodUsu { get; set; }
        public string Duracion { get; set; }
        public string PasadaSueldos { get; set; }
        public string Observacion { get; set; }
        public string Anular { get; set; }
        public int? CodDesigbsas { get; set; }

        public override string Description => this.TipoServ;
    }
}
