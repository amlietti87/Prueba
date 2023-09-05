using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{

    public partial class ReporteDistribucionCochesFilter
    {
        public List<decimal> lineList { get; set; }
        public DateTime? fecha { get; set; }
        public Nullable<int> codTdia { get; set; }
    }

    public partial class ReporteDistribucionCoches
    {

        public ReporteDistribucionCoches()
        {
            this.Galpones = new List<ReporteDistribucionCochesSubgalpones>();
            this.Horarios = new List<ReporteDistribucionHoraios>();
        }
        public List<ReporteDistribucionHoraios> Horarios { get; set; }
        public List<ReporteDistribucionCochesSubgalpones> Galpones { get; set; }
    }

    public partial class ReporteDistribucionHoraios
    {
        public int cod_hfecha { get; set; }

        public decimal cod_lin { get; set; }
        public string des_lin { get; set; }
        public string des_tdia { get; set; }
        public int cod_tdia { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public System.DateTime FechaHorario { get; set; }
        public Nullable<int> TotalCoches { get; set; }
        public string Motivo { get; set; }
        public List<ReporteDistribucionCochesSubgalpones> Galpones { get; set; }
    }

    public partial class ReporteDistribucionCochesSubgalpones
    {
        public Nullable<int> cod_hfecha { get; set; }
        public Nullable<int> CantidadCochesReal { get; set; } 
        public int? CantidadConductoresReal { get; set; }
        public int cod_tdia { get; set; }
        public string des_subg { get; set; }

        public decimal cod_subg { get; set; }

    }
}
