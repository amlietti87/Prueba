using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class InspTareaCampoDto :EntityDto<int>
    {
        public int TareaId { get; set; }
        public int TareaCampoConfigId { get; set; }
        public string Etiqueta { get; set; }
        public bool Requerido { get; set; }
        public int? Orden { get; set; }
        public override string Description => "";

        public string NombreTareaCampo { get; set; }
        public string Campo { get; set; }
    }
}
