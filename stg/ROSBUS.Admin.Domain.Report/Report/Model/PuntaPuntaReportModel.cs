using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Report.Report.Model
{
    public class PuntaPuntaReportModel
    {
        public string Empresa { get; set; }
        public string Linea { get; set; }
        public string FechaEmision { get; set; }
        public string SubGalpon { get; set; }
        public string TipoDia { get; set; }
        public string FechaHorario { get; set; }
        public string Servicio { get; set; }

        public string Sale { get; set; }
        public string Llega { get; set; }

        public string Bandera { get; set; }

        public string Minutos { get; set; }

        public int NumeroColumna { get; set; }

        public int RowColor { get; set; }
    }           
   

}
