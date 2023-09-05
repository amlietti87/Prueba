using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HDesignar : Entity<int>
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
    }



    public class HDesignarSabanaSector
    {
        public decimal CodLinea { get; set; }
        public string DescripcionLinea { get; set; }
        public int CodBandera { get; set; }
        public string AbreviacionBandera { get; set; }
        public string NumeroServicio { get; set; }
        public string DescripcionEmpleado { get; set; }
        public string LegajoEmpleado { get; set; }
        public DateTime HoraPaso { get; set; }

        public String HoraFormated
        {
            get
            {
                return this.HoraPaso.ToString(@"HH\:mm");
               
            }
        }

    }
}
