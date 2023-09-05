using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspEstadosDiagramaInspectoresDto : EntityDto<int>
    {
        //public InspEstadosDiagramaInspectores()
        //{
        //    InspDiagramasInspectores = new List<InspDiagramasInspectores>();
        //}
        
        public string Descripcion { get; set; }
        public bool EsBorrador { get; set; }


        public override string Description => this.Descripcion;

        //public List<InspDiagramasInspectoresDto> InspDiagramasInspectores { get; set; }
    }
}
