using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
     public partial class InspEstadosDiagramaInspectores : Entity<int>
    {
        public const int EstadosDiagrama01_Borrador = 1;
        public const int EstadosDiagrama02_Publicado = 2;

        public InspEstadosDiagramaInspectores()
        {
            InspDiagramasInspectores = new HashSet<InspDiagramasInspectores>();
        }
        
        public string Descripcion { get; set; }
        public bool EsBorrador { get; set; }


        public ICollection<InspDiagramasInspectores> InspDiagramasInspectores { get; set; }
    }
}
