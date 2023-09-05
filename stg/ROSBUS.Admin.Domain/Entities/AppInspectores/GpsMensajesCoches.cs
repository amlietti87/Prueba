using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class GpsMensajesCoches : Entity<int>
    {
        public int Codigo { get; set; }
        public int Origen { get; set; }
        public string Usuario { get; set; }
        public string Texto { get; set; }
        public int Id2 { get; set; }
        public int CodLin { get; set; }
        public int? CodTdia { get; set; }
        public int? Servicio { get; set; }
        public int Legajo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int? CodUsuario { get; set; }
        public DateTime? Dia { get; set; }
        public string Maquina { get; set; }
        public string Enviado { get; set; }
        public int? Ficha { get; set; }
    }
}
