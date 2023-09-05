using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class InspTareaCampo : Entity<int>
    {
        public int TareaId { get; set; }
        public int TareaCampoConfigId { get; set; }
        public string Etiqueta { get; set; }
        public bool Requerido { get; set; }
        public int? Orden { get; set; }
        public InspTarea Tarea { get; set; }
        public InspTareaCampoConfig TareaCampoConfig { get; set; }

    }
}
