using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{
    public class ReporteHorarioExcelModel
    {
        public string Linea { get; set; }

        public int CodTipoDia { get; set; }
        public String TipoDia { get; set; }

        public string Servicio { get; set; }

        public DateTime Sale { get; set; }

        public DateTime Llega { get; set; }

        public string Bandera { get; set; }

        public string TipoDeHora { get; set; }

        public Decimal Duracion { get; set; }

        public string SubGalpon { get; set; }

        public DateTime FechaHorario { get; set; }


        public TimeSpan SaleFormat
        {

            get
            {
                return new TimeSpan(this.Sale.Day-1, this.Sale.Hour, this.Sale.Minute, this.Sale.Second);
            }
        }

        public TimeSpan LlegaFormat
        {

            get
            {
                return new TimeSpan(this.Llega.Day-1, this.Llega.Hour, this.Llega.Minute, this.Llega.Second);
            }
        }

        public int? ServicioFormat
        {

            get
            {
                int nroServicio;

                if (int.TryParse(this.Servicio.TrimStart('0'), out nroServicio))
                {
                    return nroServicio;
                }

                return null;
            }
        }


    }
}
