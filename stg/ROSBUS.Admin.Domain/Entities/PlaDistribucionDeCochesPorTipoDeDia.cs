using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaDistribucionDeCochesPorTipoDeDia : Entity<int>
    {
        public PlaDistribucionDeCochesPorTipoDeDia()
        {
            this.Banderas = new List<int>();
        }

        // public int Id { get; set; }
        public int CodHfecha { get; set; }
        public string Motivo { get; set; }
        public int CodTdia { get; set; }
        public int CantidadDeCochesEstimados { get; set; }
        public int? CantidadDeConductoresEstimados { get; set; }

       

        public HFechasConfi CodHfechaNavigation { get; set; }
        public HTipodia CodTdiaNavigation { get; set; }


        public string Descripcion { get; set; }

        [NotMapped]
        public List<int> Banderas { get; set; }
    }


    public partial class ImportarServiciosInput
    {
        public ImportarServiciosInput()
        {
            this.BolBanderasCartel = new BolBanderasCartelDto();
        }

        [Required]
        public string PlanillaId { get; set; }

        [Required]
        public int? CodHfecha { get; set; }
        [Required]
        public int? CodLinea { get; set; }
        [Required]
        public int? CodTdia { get; set; }
        public int? desde { get; set; }
        public int? hasta { get; set; }
        public BolBanderasCartelDto BolBanderasCartel { get; set; }
        public List<HMediasVueltasImportadaDto> MediasVueltas { get; set; }
    }



    public partial class PlaDistribucionEstadoView {

        public PlaDistribucionEstadoEnum Estado { get; set; }
    }

    public enum PlaDistribucionEstadoEnum {
        Incompleta = 0,
        SinMinutos = 1,
        Valido = 2
    }

    public partial class PlaDuracionesEstadoView 
    {
        public PlaDuracionesEstadoEnum Estado { get; set; }
    }

    public enum PlaDuracionesEstadoEnum
    {
        Incompleta = 0,
        Completo = 1
    }


}
