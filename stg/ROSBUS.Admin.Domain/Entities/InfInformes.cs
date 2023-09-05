using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
     public class InfInformes : Entity<string>
    {
        //public string CodInforme { get; set; }
        public DateTime? FecInforme { get; set; }
        public DateTime? FecInfraccion { get; set; }
        public string CodMotivo { get; set; }
        public string CodInspector { get; set; }
        public string DscLugar { get; set; }
        public string ObsInforme { get; set; }
        public string CodEmp { get; set; }
        public int? CodEmpr { get; set; }
        public int? CodLin { get; set; }
        public string NroInterno { get; set; }
        public string NumSer { get; set; }
        public int? CodUsuinf { get; set; }
        public string CodRespinf { get; set; }
        public int? Imprimio { get; set; }
        public DateTime? FecAnulacion { get; set; }
        public string ObsAnulacion { get; set; }
        public string CodRespanulacion { get; set; }
        public int? CodUsuanulacion { get; set; }
        public int? CodUsuarioAnulacion { get; set; }
        public int? ControlSancion { get; set; }
        public DateTime? FecSancion { get; set; }
        public string ObsSancion { get; set; }
        public int? DiasSancion { get; set; }
        public string CodRespsancion { get; set; }
        public int? CodUsusancion { get; set; }
        public DateTime? FecModsancion { get; set; }
        public string ObsModsancion { get; set; }
        public string CodModrespsancion { get; set; }
        public int? CodUsumodsancion { get; set; }
        public DateTime? FecCartadocumento { get; set; }
        public string ObsCartadocumento { get; set; }
        public string NroCartadocumento { get; set; }
        public string CodRespcarta { get; set; }
        public int? CodUsucarta { get; set; }
        public string TpoSancion { get; set; }
        public string TpoModsancion { get; set; }
        public int? DiasModsancion { get; set; }
        public int? Notificado { get; set; }
        public int? Vencido { get; set; }
        public string NroInforme { get; set; }
        public string TpoInforme { get; set; }
        public DateTime? FecIniciosancion { get; set; }
        public DateTime? FecModiniciosancion { get; set; }
        public string Sancion { get; set; }
        public string OrigenInforme { get; set; }
        public int? CodGalpon { get; set; }
        public string Anulado { get; set; }
        public int? CodSucursalInspector { get; set; }
        public string CodInformeBa { get; set; }
        public int? LugarHecho { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public DateTime? FechaNotificado { get; set; }
    }


    public class InformeConsulta
    {
        public string fechaInforme { get; set; }
        public string fechaInfraccion { get; set; }
        public string naConductor { get; set; }
        public string legConductor { get; set; }
        public string descMotivo { get; set; }
        public string descLinea { get; set; }
        public string servicio { get; set; }
        public string descLugar { get; set; }
        public string ficha { get; set; }
        public string interno { get; set; }
        public string dominio { get; set; }
        public string notificado { get; set; }
        public string fechaNotificado { get; set; }
        public string obsInforme { get; set; }
        public string numInforme { get; set; }

    }
}
