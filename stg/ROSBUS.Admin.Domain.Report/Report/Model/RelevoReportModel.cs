using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Report.Report.Model
{
    public class RelevoReportModel
    {
        public string num_ser { get; set; }

        public string DescripcionSector { get; set; }

        
        public DateTime? SaleRelevo { get; set; }
        public DateTime? LlegaRelevo { get; set; }
        public DateTime? SaleAux { get; set; }
        public DateTime? LlegaAux { get; set; }
        public DateTime? Horario { get; set; }
        public DateTime? HorarioAux { get; set; }
        public string Bandera { get; set; }
        public string BanderaAux { get; set; }

        public string Linea { get; set; }
        public string Empresa { get; set; }
        public string SubGalpon { get; set; }
        public string TipoDia { get; set; }

        public DateTime? FechaHorario { get; set; }


        public string RelevoRealRel
        {
            get
            {
                return this.SaleRelevo?.ToString("HH:mm");
            }
        }

        public string BanderaRel
        {
            get
            {
                return this.Bandera;
            }
        }

        public string HorarioFormatRel
        {
            get
            {
                return this.Horario?.ToString("HH:mm:ss");
            }
        }

        public string DescripcionSectorRel
        {
            get
            {
                return this.DescripcionSector;
            }
        }
        public string DescripcionSectorAux { get; set; }

        public string RelevoRealAux
        {
            get
            {
                if (this.SaleAux.HasValue)
                {
                    return this.SaleAux?.ToString("HH:mm");
                }

                return "";
            }
        }


        public string HorarioFormatAux
        {
            get
            {
                if (this.SaleAux.HasValue)
                {
                    return this.HorarioAux?.ToString("HH:mm:ss");
                }
                return "";
            }
        }



    }
}
