using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class InspTareasRealizadas :   FullAuditedEntity<int>
    {


        public InspTareasRealizadas()
        {
            InspTareasRealizadasDetalle = new HashSet<InspTareasRealizadasDetalle>();
        }


        public DateTime Fecha { get; set; }
        public int EmpleadoId { get; set; }
        public int? SucursalId { get; set; }
        public int TareaId { get; set; }


        public InspTarea Tarea { get; set; }

        public ICollection<InspTareasRealizadasDetalle> InspTareasRealizadasDetalle { get; set; }
    }
}
