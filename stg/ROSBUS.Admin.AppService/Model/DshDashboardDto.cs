using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class DshDashboardDto :EntityDto<int>
    {

        public override string Description => this.Descripcion;

        public string Descripcion { get; set; }
        public int TipoDashboardId { get; set; }
    }



    public class UsuarioDashboardItemDto : EntityDto<int>
    {
        public int DashboardId { get; set; }
        public int CodUsuario { get; set; }
        public int Columna { get; set; }
        public int Orden { get; set; }


        public int? TipoDashboardId { get; set; }


        public override string Description => "";
    }


    public class UsuarioDashboardInput
    {
        public UsuarioDashboardInput()
        {
            this.Items = new List<UsuarioDashboardItemDto>();
        }

        public List<UsuarioDashboardItemDto> Items { get; set; }

        public int DashboardLayoutId { get; set; }


    }
}
