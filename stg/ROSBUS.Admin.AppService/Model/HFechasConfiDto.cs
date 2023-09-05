using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HFechasConfiDto : EntityDto<int>
    {
        public HFechasConfiDto()
        {
            this.HBasec = new List<HBasecDto>();
            this.DistribucionDeCochesPorTipoDeDia = new List<PlaDistribucionDeCochesPorTipoDeDiaDto>();
        }


        public override string Description => this.FechaDesde.ToString("dd/MM/yyyy");

        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal CodLinea { get; set; }

        [Required]
        public int? PlaEstadoHorarioFechaId { get; set; }


        public string TiposDeDias { get; set; }


        public string DescripcionLinea { get; set; }
        public string DescripcionEstado { get; set; }
        public bool? BeforeMigration { get; set; }
        public ItemDto<Decimal> Linea { get; set; }


        public List<PlaDistribucionDeCochesPorTipoDeDiaDto> DistribucionDeCochesPorTipoDeDia { get; set; }

        public List<HBasecDto> HBasec { get; set; }
    }

    public class PlaDistribucionDeCochesPorTipoDeDiaDto : EntityDto<int>
    {
        public override string Description => "";

        public int CodHfecha { get; set; }
        public string Motivo { get; set; }
        public int CodTdia { get; set; }
        public int CantidadDeCochesEstimados { get; set; }
        public int? CantidadDeConductoresEstimados { get; set; }
        public string TipoDediaDescripcion { get; set; }
        public string Descripcion { get; set; }
    }


   





}
