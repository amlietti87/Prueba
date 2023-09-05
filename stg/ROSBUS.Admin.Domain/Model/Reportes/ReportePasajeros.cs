using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{

    public partial class ReportePasajerosFilter
    {
        public decimal LineaId { get; set; }
        public int[] Banderas { get; set; }
    }

    public partial class ReportePasajeros
    {

        public ReportePasajeros()
        {
            this.Detalle = new List<DetalleReportePasajeros>();
        }

        public List<DetalleReportePasajeros> Detalle { get; set; }

    }
    public partial class DetalleReportePasajeros
    {
        public decimal LineaId { get; set; }
        public Int64 RamalId { get; set; }
        public int BanderaId { get; set; }
        public string Linea { get; set; }
        public string Ramal { get; set; }
        public string Bandera { get; set; }
        public string DescripcionPasajeros { get; set; }
        public string CodigoParada { get; set; }
        public string Localidad { get; set; }
        public string Calle { get; set; }
        public string Cruce { get; set; }
        public string NombreMapa { get; set; }
    }

    public partial class ReporteParadasRuta
    {

        public ReporteParadasRuta()
        {
            this.DetalleParadas = new List<DetalleReporteParadasRutas>();
        }

        public List<DetalleReporteParadasRutas> DetalleParadas { get; set; }

    }
    public partial class DetalleReporteParadasRutas
    {
        public decimal LineaId { get; set; }
        public Int64 RamalId { get; set; }
        public int BanderaId { get; set; }
        public string Linea { get; set; }
        public string Ramal { get; set; }
        public string Bandera { get; set; }
        public string DescripcionPasajeros { get; set; }
        public string CodigoParada { get; set; }
        public string Localidad { get; set; }
        public string Calle { get; set; }
        public string Cruce { get; set; }
        public string NombreMapa { get; set; }

    }

}
