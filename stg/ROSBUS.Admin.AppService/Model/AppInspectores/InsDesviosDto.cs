using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public partial class InsDesviosDto : EntityDto<long>
    {
        public DateTime FechaHora { get; set; }
        public string FechaString {
            get
            {
                return this.FechaHora.ToString("dd/MM/yyyy HH:mm");
            }
            set
            {
                DateTime fec;
                if (DateTime.TryParse(value, out fec))
                {
                    this.FechaHora = fec;
                }              

            }
        }
        public string Descripcion { get; set; }
        public int SucursalId { get; set; }
        public bool Leido { get; set; }

        public override string Description => Descripcion;
    }
}
