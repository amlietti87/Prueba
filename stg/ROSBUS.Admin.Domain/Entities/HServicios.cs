using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HServicios : Entity<int>
    {
        public HServicios()
        {
            HMediasVueltas = new HashSet<HMediasVueltas>();
        }

        public int CodHconfi { get; set; }
        public string NumSer { get; set; }
        //public int CodServicio { get; set; }
        public string NroInterno { get; set; }
        public int Duracion { get; set; }

        public HHorariosConfi CodHconfiNavigation { get; set; }
        public ICollection<HMediasVueltas> HMediasVueltas { get; set; }
    }


    public class ConductoresLegajoDto : ItemDto<string>
    {
        public string Legajo { get; set; }

        public string Dni { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaAntiguedad { get; set; }

        public int CodEmpresa { get; set; }

        public string DesEmpresa { get; set; }

        public string Cuil { get; set; }

        public String FechaIngresoFormated
        {
            get
            {
                return this.FechaIngreso.ToString(@"dd/MM/yyyy");

            }
        }

        public String FechaAntiguedadFormated
        {
            get
            {
                return this.FechaAntiguedad.ToString(@"dd/MM/yyyy");

            }
        }
    }

}
