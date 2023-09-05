using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HFechasConfi : Entity<int>
    {
        public HFechasConfi()
        {
            HBasec = new HashSet<HBasec>();
            HHorariosConfi = new HashSet<HHorariosConfi>();
            HMinxtipo = new HashSet<HMinxtipo>();

            PlaDistribucionDeCochesPorTipoDeDia = new HashSet<PlaDistribucionDeCochesPorTipoDeDia>();
        }

        //public override int Id
        //{
        //    get => this.CodHfecha;
        //    set => this.CodHfecha = value;
        //}

        //public int CodHfecha { get; set; }

        public DateTime FecDesde { get; set; }
        public DateTime? FecHasta { get; set; }
        public decimal CodLin { get; set; }
        public string Activo { get; set; }
        public decimal? CantCoches { get; set; }
        public int? CodSabanaHoraria { get; set; }
        public int? PlaEstadoHorarioFechaId { get; set; }
        public bool? BeforeMigration { get; set; }
        public Linea CodLinNavigation { get; set; }
        public PlaEstadoHorarioFecha PlaEstadoHorarioFecha { get; set; }
        public ICollection<HBasec> HBasec { get; set; }
        public ICollection<HHorariosConfi> HHorariosConfi { get; set; }
        public ICollection<HMinxtipo> HMinxtipo { get; set; }

        public ICollection<PlaDistribucionDeCochesPorTipoDeDia> PlaDistribucionDeCochesPorTipoDeDia { get; set; }
    }



    public class PlaHorarioFechaLineaListView
    {

        public decimal CodLinea { get; set; }

        public string DescripcionLinea { get; set; }

        public DateTime? FechaUltimaModificacion { get; set; }
        public Boolean Activo { get; set; }



    }


    public class DestinatarioMail
    {
        public string destinatariosMail { get; set; }
    }




    public class validarTimeLineHorarioResult
    {

        public int SolapaDesde { get; set; }
        public int SolapaHasta { get; set; }
        public int SolapaFuera { get; set; }
        public int SolapaDentro { get; set; }
        public int Sigiente { get; set; }
        public int cod_hfecha { get; set; }
        public DateTime fec_desde { get; set; }
        public DateTime? fec_hasta { get; set; }
        public decimal cod_lin { get; set; }
    }

}
