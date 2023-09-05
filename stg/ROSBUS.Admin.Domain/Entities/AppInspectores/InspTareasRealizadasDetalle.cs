using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class InspTareasRealizadasDetalle : Entity<int>
    {
        public int TareaRealizadaId { get; set; }
        public int TareaCampoId { get; set; }
        public string Valor { get; set; }


        public InspTareaCampo TareaCampo { get; set; }
        public InspTareasRealizadas TareaRealizada { get; set; }
    }
}
