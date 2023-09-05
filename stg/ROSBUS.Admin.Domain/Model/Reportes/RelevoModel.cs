using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model.Reportes
{
    public class RelevoModel
    {
        public string num_ser { get; set; }

        public string DescripcionSector { get; set; }

        public DateTime? SaleTitular { get; set; }
        public DateTime? SaleRelevo { get; set; }
        public DateTime? LlegaRelevo { get; set; }
        public DateTime? SaleAux { get; set; }
        public DateTime? LlegaAux { get; set; }
        public DateTime? Horario { get; set; }

        public string Bandera { get; set; }
        public string Linea { get; set; }
        public string Empresa { get; set; }
        public string SubGalpon { get; set; }
        public string TipoDia { get; set; }

        public DateTime? FechaHorario { get; set; }
        public string BanderaAux { get; set; }
        public string DescripcionSectorAux { get; set; }
        public DateTime? HorarioAux { get; set; }
    }
}
