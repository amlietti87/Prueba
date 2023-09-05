using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class BolSectoresTarifariosDto: EntityDto<int>
    {
        //public int CodSectorTarifario { get; set; }
        public string DscSectorTarifario { get; set; }
        public int CodManualSectorTarifario { get; set; }
        public string DscCompleta { get; set; }
        public bool? Nacional { get; set; }
        public int? CodSectorTarifariobsas { get; set; }

        public override string Description => DscCompleta;
    }
}
