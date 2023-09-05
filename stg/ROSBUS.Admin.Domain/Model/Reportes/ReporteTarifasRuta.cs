using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{

    public partial class ReporteTarifasRuta
    {

        public ReporteTarifasRuta()
        {
            this.TarifasPlanasRespaldo = new List<TarifasPlanasRespaldo>();
            this.TarifasTrianguloRespaldo = new List<TarifasTrianguloRespaldo>();
        }

        public List<TarifasPlanasRespaldo> TarifasPlanasRespaldo { get; set; }
        public List<TarifasTrianguloRespaldo> TarifasTrianguloRespaldo { get; set; }

    }
    public partial class TarifasPlanasRespaldo
    {
        public decimal Importe { get; set; }
        public int MedioPago { get; set; }
        public int CodLin { get; set; }
        public string DescripcionCiudad { get; set; }
        public string DescripcionTipoBoleto { get; set; }
        public int SectorTarifario { get; set; }
        public DateTime FechaActivacion { get; set; }
        public string Linea { get; set; }
    }

    public partial class TarifasTrianguloRespaldo
    {
        public decimal Importe { get; set; }
        public int MedioPago { get; set; }
        public int CodLin { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public int SectorTarDesde { get; set; }
        public int SectorTarHasta { get; set; }
        public DateTime FechaActivacion { get; set; }
        public string Linea { get; set; }

    }

}
