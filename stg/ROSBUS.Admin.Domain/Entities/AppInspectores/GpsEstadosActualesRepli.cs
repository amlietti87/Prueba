using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class GpsEstadosActualesRepli
    {
        public int Id { get; set; }
        public string Legajo { get; set; }
        public string Alerta { get; set; }
        public int? Codlin { get; set; }
        public string Deslin { get; set; }
        public int? CodHfecha { get; set; }
        public int? Codtip { get; set; }
        public string Destip { get; set; }
        public int? Servi { get; set; }
        public int? Proceso { get; set; }
        public int? CodBan { get; set; }
        public string Desban { get; set; }
        public int? Ficha { get; set; }
        public string Activo { get; set; }
        public string Fechai { get; set; }
        public string Horai { get; set; }
        public float? Lat { get; set; }
        public float? Lon { get; set; }
        public float? Late { get; set; }
        public float? Lone { get; set; }
        public string Sdesde { get; set; }
        public string Shasta { get; set; }
        public int? Cdesde { get; set; }
        public int? Chasta { get; set; }
        public string Hdesde { get; set; }
        public string Hhasta { get; set; }
        public float? Kdesde { get; set; }
        public int? Cuenta { get; set; }
        public float? Sent { get; set; }
        public string Sentido { get; set; }
        public float? Veloc { get; set; }
        public int? Pendiente { get; set; }
        public string Esta { get; set; }
        public string Sectores { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Atraso { get; set; }
        public int? Pedido { get; set; }
        public int? Celular { get; set; }
        public int? Indliscon { get; set; }
        public int? CodEstado { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? ComienzoEstado { get; set; }
        public string Diagramado { get; set; }
        public string Observaciones { get; set; }
        public int? NroServ { get; set; }
        public string Ip { get; set; }
        public DateTime? FechaHora { get; set; }
        public int TpoPc { get; set; }
        public int? CodMediaVuelta { get; set; }
        public string Interno { get; set; }
        public string CodEmp { get; set; }
    }
}
