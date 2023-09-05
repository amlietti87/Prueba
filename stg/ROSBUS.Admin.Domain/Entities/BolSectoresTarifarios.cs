using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class BolSectoresTarifarios: Entity<int>
    {
        public BolSectoresTarifarios()
        {
            //BolTriangulosSectores = new HashSet<BolTriangulosSectores>();
            //BolZonasSectores = new HashSet<BolZonasSectores>();
            PlaPuntos = new HashSet<PlaPuntos>();
            HSectores = new HashSet<HSectores>();
        }

        //public int CodSectorTarifario { get; set; }
        public string DscSectorTarifario { get; set; }
        public int CodManualSectorTarifario { get; set; }
        public string DscCompleta { get; set; }
        public bool? Nacional { get; set; }
        public int? CodSectorTarifariobsas { get; set; }
        public ICollection<PlaPuntos> PlaPuntos { get; set; }

        //public ICollection<BolTriangulosSectores> BolTriangulosSectores { get; set; }
        //public ICollection<BolZonasSectores> BolZonasSectores { get; set; }
        public ICollection<HSectores> HSectores { get; set; }
    }
}
